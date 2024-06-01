using Common.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;


namespace SQLRepository.Repository
{
    public class CosmosRepository : RepositoryInterface
    {

        private readonly CosmosClient _cosmosClient;
        private readonly Database _database;
        private readonly Container _container;

        public CosmosRepository(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
            _database = _cosmosClient.GetDatabase("StudentsDB");
            _container = _database.GetContainer("test");
        }
        public async Task<List<Users>> GetStudents()
        {
            try
            {
                // Get the type name of the object
                string objectType = typeof(Users).Name;
               

                // Define a query to filter posts by object type
                QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.type = @Type")
                    .WithParameter("@Type", objectType);

                List<Users> users = new List<Users>();

                // Execute the query to retrieve items
                using (FeedIterator<Users> resultSet = _container.GetItemQueryIterator<Users>(queryDefinition))
                {
                    while (resultSet.HasMoreResults)
                    {
                        FeedResponse<Users> response = await resultSet.ReadNextAsync();
                        users.AddRange(response);
                    }
                }

                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }

        public async Task<Users> GetStudentById(string userId)
        {

            try
            {
                // Define a query to filter users by user_id
                QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.user_id = @UserId AND c.type = 'Users'")
                    .WithParameter("@UserId", userId);

                Users user = null;

                // Execute the query to retrieve users by user_id
                using (FeedIterator<Users> resultSet = _container.GetItemQueryIterator<Users>(queryDefinition))
                {
                    while (resultSet.HasMoreResults)
                    {
                        FeedResponse<Users> response = await resultSet.ReadNextAsync();
                        user = response.FirstOrDefault();
                        // We assume there's only one user with the specified user_id, so we only take the first result.
                        // If there can be multiple users with the same ID, you may need to handle this differently.
                    }
                }

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }



        //public async Task<List<Post>> GetAllPosts()
        //{
        //    var query = _container.GetItemLinqQueryable<Users>().SelectMany(user => user.posts).ToFeedIterator();
        //    var posts = new List<Post>();
        //    while (query.HasMoreResults)
        //    {
        //        var response = await query.ReadNextAsync();
        //        posts.AddRange(response);
        //    }
        //    posts = posts.OrderByDescending(post => post.created).ToList();
        //    return posts;
        //    //throw new NotImplementedException();
        //}

        
        public async Task<List<Post>> GetPostsById(string userId)
        {
            try
            {
                // Get the type name of the object
                string objectType = new Post(userId).GetType().Name;

                // Define a query to filter posts by user_id and object type
                QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c WHERE  c.type = 'Post' AND c.user_id=@userId")
                    //.WithParameter("@UserId", userId)
                    .WithParameter("@userId", userId);

                List<Post> userPosts = new List<Post>();

                // Execute the query to retrieve posts by user_id and object type
                using (FeedIterator<Post> resultSet = _container.GetItemQueryIterator<Post>(queryDefinition))
                {
                    while (resultSet.HasMoreResults)
                    {
                        FeedResponse<Post> response = await resultSet.ReadNextAsync();
                        userPosts.AddRange(response);
                    }
                }

                return userPosts;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task InsertPost(Post post)
        {
            Console.WriteLine("cos", post);
            try
            {
                await _container.CreateItemAsync(post);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("cos", post.title);
        }

        public async Task InsertStudent(Users student)
        {
            string HashPassword(string password)
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }

            string hashedPassword = HashPassword(student.password);
            student.password = hashedPassword;

            var query = new QueryDefinition("SELECT * FROM c WHERE c.name = @name AND c.type='Users'")
                            .WithParameter("@name", student.name);
            var iterator = _container.GetItemQueryIterator<dynamic>(query);
            var existingDocument = (await iterator.ReadNextAsync()).FirstOrDefault();

            if (existingDocument != null)
            {
                // Username already exists, handle accordingly (throw exception, log message, etc.)
                // For example, you can throw an exception
                //Console.WriteLine(existingDocument.id);
                throw new InvalidOperationException("Username already exists.");
            }
            else
            {
                await _container.CreateItemAsync(student);
            }
        }
        public async Task<Users> Login(string name, string password)
        {
            // Function to hash the password using SHA-256
            string HashPassword(string password)
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }

            // Check if a document with the provided name exists
            var query = new QueryDefinition("SELECT * FROM c WHERE c.name = @name AND c.type='Users'")
                            .WithParameter("@name", name);
            var iterator = _container.GetItemQueryIterator<dynamic>(query);
            var existingDocument = (await iterator.ReadNextAsync()).FirstOrDefault();

            if (existingDocument != null)
            {
                // Username exists, check if the password matches
                string hashedPassword = HashPassword(password);
                string storedPassword = existingDocument["password"].ToString();
                if (hashedPassword == storedPassword)
                {
                    // Password matches, return login success message
                    return existingDocument.ToObject<Users>();
                }
                else
                {
                    // Password does not match, return login failed message
                    throw new ArgumentException("Incorrect password.");
                }
            }
            else
            {
                // Username does not exist, return username not exist message
                throw new KeyNotFoundException("Username does not exist.");
            }
        }

        public async Task UpdateStudent(Users student)
        {
            await _container.UpsertItemAsync(student, new PartitionKey(student.id));
        }
        public async Task DeleteStudent(string name)
        {
            await _container.DeleteItemAsync<Users>(name, new PartitionKey(name));
        }
        public async Task InsertComment(Comment comment)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @post_id AND c.type='Post'")
                          .WithParameter("@post_id", comment.post_id);
            var iterator = _container.GetItemQueryIterator<Post>(query);
            Post post = (await iterator.ReadNextAsync()).FirstOrDefault();
            if (post != null)
            {
                post.comments.Add(comment);
            }
            await _container.UpsertItemAsync(post, new PartitionKey(post.user_id));
        }
        public async Task Likes(string post_id, string user_id)
        {
           
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @post_id AND c.type='Post'")
                          .WithParameter("@post_id", post_id);
            var iterator = _container.GetItemQueryIterator<dynamic>(query);
            var postObj= (await iterator.ReadNextAsync()).FirstOrDefault();
            Console.WriteLine(user_id);
            if (postObj != null)
            {
                var post = postObj.ToObject<Post>();
                if (post.likes == null)
                {
                    post.likes = new List<string>();
                }
                if (post.likes.Contains(user_id))
                {
                    post.likes.Remove(user_id);
                }
                else
                    post.likes.Add(user_id);



                string x = post.user_id;

                await _container.UpsertItemAsync(post, new PartitionKey(x));
            }
        }
    }
}

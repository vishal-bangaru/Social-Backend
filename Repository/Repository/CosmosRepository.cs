using Common.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
  

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
            var query = _container.GetItemLinqQueryable<Users>().ToFeedIterator();
            var std = new List<Users>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                Console.WriteLine(response.ToString());
                std.AddRange(response);
            }
            return std;
            //throw new NotImplementedException();
        }

        public async Task<Users> GetStudentById(string name)
        {
            try
            {
                var response = await _container.ReadItemAsync<Users>(name, new PartitionKey(name));
                return response.Resource;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }
        public async Task<List<Post>> GetAllPosts()
        {
            var query = _container.GetItemLinqQueryable<Users>().SelectMany(user => user.posts).ToFeedIterator();
            var posts = new List<Post>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                posts.AddRange(response);
            }
            posts = posts.OrderByDescending(post => post.created).ToList();
            return posts;
            //throw new NotImplementedException();
        }

        public async Task InsertPost(Post post, Users user)
        {
            user.posts.Add(post);
            await _container.UpsertItemAsync(user, new PartitionKey(user.name));
        }

        public async Task InsertStudent(Users student)
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

            // Hash the password before storing it
            string hashedPassword = HashPassword(student.password);
            student.password = hashedPassword;

            // Check if a document with the same username already exists
            var query = new QueryDefinition("SELECT * FROM c WHERE c.name = @name")
                            .WithParameter("@name", student.name);
            var iterator = _container.GetItemQueryIterator<dynamic>(query);
            var existingDocument = (await iterator.ReadNextAsync()).FirstOrDefault();

            if (existingDocument != null)
            {
                // Username already exists, handle accordingly (throw exception, log message, etc.)
                // For example, you can throw an exception
                throw new InvalidOperationException("Username already exists.");    
            }
            else
            {
                await _container.CreateItemAsync(student);
            }
        }
        public async Task<string> Login(string name, string password)
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
            var query = new QueryDefinition("SELECT * FROM c WHERE c.name = @name")
                            .WithParameter("@name", name);
            var iterator = _container.GetItemQueryIterator<dynamic>(query);
            var existingDocument = (await iterator.ReadNextAsync()).FirstOrDefault();

            if (existingDocument != null)
            {
                // Username exists, check if the password matches
                string hashedPassword = HashPassword(password);
                string storedPassword = existingDocument["password"].ToString();
                if (hashedPassword==storedPassword)
                {
                    // Password matches, return login success message
                    return "Login successful.";
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
    }
}
using Common.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLRepository.Repository
{
    public interface RepositoryInterface
    {
        public Task<List<Users>> GetStudents();
        public Task<Users> GetStudentById(string id);
        public Task InsertStudent(Users student);
        public Task InsertPost(Post post);
        public Task UpdateStudent(Users student);
        public Task DeleteStudent(string id);
        public Task<Users> Login(string name, string password);
        public Task<List<Post>> GetPostsById(string id);
        public Task InsertComment(Comment comment);
        public Task Likes(string post_id, string user_id);
    }
}
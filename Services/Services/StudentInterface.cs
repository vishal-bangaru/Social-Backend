using Microsoft.Azure.Cosmos;
using PBLClass.Service;
using Common.Models;
using Microsoft.AspNetCore.Http;
using SQLRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBLClass.Service
{
    public interface StudentInterface
    {
        public Task<List<Users>> GetStudentNames();
        public Task<Users> GetStudentById(string id);
        public Task Insert(Users student);
        public Task Update(Users student);
        public Task Delete(string id);
        public Task PostImg(IFormFile file, string id,string content,string owner);
        public Task<Users> Login(string name, string password);
        public Task<List<Post>> GetPostsById(string userId);
        public Task InsertComment(Comment comment);
        public Task Likes(string post_id, string user_id);
        public Task Request(string cur_id, string req_id);
        public Task Approve(string cur_id, string req_id);
  
    }
}
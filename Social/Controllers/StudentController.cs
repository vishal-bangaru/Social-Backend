
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Http;
using PBLClass.Service;
using Common.Models;
using Common.Services;
using Azure.Core;

namespace App1.Controllers
{

    [ApiController]
    [Route("students")]
    public class studentsController : ControllerBase
    {


        private readonly StudentInterface _studentInterface;
        private readonly CloudinaryService _cloudinaryService;

        public studentsController(StudentInterface studentInterface, CloudinaryService cloudinaryService)
        {
            _studentInterface = studentInterface;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        [Route("GetAllStudents")]
        public async Task<List<Users>> Get()
        {
            return await _studentInterface.GetStudentNames();
        }
        [HttpGet]
        [Route("GetStudentById")]
        public async Task<Users> Get(string id)
        {
            return await _studentInterface.GetStudentById(id);
        }

        [HttpPost]
        [Route("InsertStudent")]
        public async Task Insert(Users student)
        {
            await _studentInterface.Insert(student);
           
        }

        [HttpPut]
        [Route("UpdateStudent")]
        public async Task Update(Users student)
        {
            await _studentInterface.Update(student);
        }

        [HttpDelete]
        [Route("DeleteStudent")]
        public async Task Delete(Users student)
        {
            await _studentInterface.Delete(student.user_id);
        }

        [HttpPost]
        [Route("PostImg")]
        public async Task PostImg() // file, userid
        {
            try
            {
                var file = Request.Form.Files["file"];
                
                if (file == null) return;
                string id = Request.Form["user_id"].ToString();
                string content = Request.Form["content"].ToString();
                string owner = Request.Form["owner"].ToString();
                Console.WriteLine("id " + id);
                await _studentInterface.PostImg(file, id,content,owner);
            }
            catch (Exception ex)
            {
                return;
            }
        }
        [HttpPost]
        [Route("Login")]
        public async Task<Users> Login(Users student)
        {
            return await _studentInterface.Login(student.name, student.password);
        }
        [HttpGet]
        [Route("GetPostsById")]
        public async Task<List<Post>> GetPostsById(string userId)
        {
            return await _studentInterface.GetPostsById(userId);
        }
        [HttpPost]
        [Route("InsertComment")]
        public async Task InsertComment(Comment comment)
        {
            await _studentInterface.InsertComment(comment);
        }
        [HttpPost]
        [Route("Likes")]
        public async Task Likes()
        {
            string post_id = Request.Form["post_id"].ToString(); 
            string user_id = Request.Form["user_id"].ToString();
            
         // Console.WriteLine(post_id  + "\n " + user_id);    
            await _studentInterface.Likes(post_id,user_id);
        }
        [HttpPost]
        [Route("Request")]
        public async Task FollowRequest(string cur_id, string req_id)
        {
            try
            {
                await _studentInterface.Request(cur_id, req_id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        [HttpPost]
        [Route("Approve")]

        public async Task Approve(string cur_id, string app_id)
        {
            try
            {
                await _studentInterface.Approve(cur_id, app_id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


    }
}
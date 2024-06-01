using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Services.Services;
using SQLRepository.Repository;

namespace PBLClass.Service
{
    public class StudentService : StudentInterface
    {
        private readonly RepositoryInterface _studentRepository;
        private readonly CloudinaryService _cloudinaryService;
        private readonly FileServiceInterface _fileService;
        List<string> students = new List<string>();
        public StudentService(RepositoryInterface studentRepository, CloudinaryService cloudinaryService,
            FileServiceInterface fileService)
        {
            _studentRepository = studentRepository;
            _cloudinaryService = cloudinaryService;
            _fileService = fileService;
        }

        public async Task<List<Users>> GetStudentNames()
        {
            return await _studentRepository.GetStudents();
        }

        public async Task<Users> GetStudentById(string id)
        {
            return await _studentRepository.GetStudentById(id);
        }

        public async Task Insert(Users student)
        {
            student.type = "Users";
            student.user_id = Guid.NewGuid().ToString();
            student.id = student.user_id;
            student.followers = new List<string>();
            student.following = new List<string>();
            await _studentRepository.InsertStudent(student);
        }

        public async Task Update(Users student)
        {
            await _studentRepository.UpdateStudent(student);
        }

        public async Task Delete(string id)
        {
            await _studentRepository.DeleteStudent(id);
        }

        public async Task PostImg(IFormFile file, string id, string content, string owner)
        {

            //Users user = await _studentRepository.GetStudentByName(id);
            
            if (file != null)
            {
                
                string imageUrl = await _fileService.Upload(file);  
                Console.WriteLine("video "+ imageUrl);
                Post post = new Post(id);
                post.post = imageUrl;
                post.content = content;
                post.title = file.FileName;
                post.user_id=id;
                post.owner=owner;
                


                await _studentRepository.InsertPost(post);
            }
            
        }
        public async Task<Users> Login(string name, string password)
        {
            return await _studentRepository.Login(name, password);
        }
        
        public async Task<List<Post>> GetPostsById(string userId)
        {
            return await _studentRepository.GetPostsById(userId);   
        }

        public async Task InsertComment( Comment comment)
        {
             await _studentRepository.InsertComment(comment);
        }
        
        public async Task Likes(string post_id,string user_id)
        {
            await _studentRepository.Likes(post_id,user_id);   
        }
        public async Task Request(string cur_id, string req_id)
        {
            try
            {
                Users cur_user = await _studentRepository.GetStudentById(cur_id);
                cur_user.requests.Add(req_id);
                await _studentRepository.UpdateStudent(cur_user);
                Users req_user = await _studentRepository.GetStudentById(req_id);
                req_user.approvals.Add(cur_id);
                await _studentRepository.UpdateStudent(req_user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public async Task Approve(string cur_id, string app_id)
        {
            try
            {
                Users cur_user = await _studentRepository.GetStudentById(cur_id);
                cur_user.approvals.Remove(app_id);
                cur_user.followers.Add(app_id);
                await _studentRepository.UpdateStudent(cur_user);
                Users app_user = await _studentRepository.GetStudentById(app_id);
                app_user.requests.Remove(cur_id);
                app_user.following.Add(cur_id);
                await _studentRepository.UpdateStudent(app_user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


    }
}
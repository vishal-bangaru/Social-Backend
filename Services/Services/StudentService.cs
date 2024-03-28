using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Http;
using SQLRepository.Repository;

namespace PBLClass.Service
{
    public class StudentService : StudentInterface
    {
        private readonly RepositoryInterface _studentRepository;
        private readonly CloudinaryService _cloudinaryService;
        List<string> students = new List<string>();
        public StudentService(RepositoryInterface studentRepository, CloudinaryService cloudinaryService)
        {
            _studentRepository = studentRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<List<Users>> GetStudentNames()
        {
            return await _studentRepository.GetStudents();
        }

        public async Task Insert(Users student)
        {
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

        public async Task PostImg(IFormFile file, string id)
        {
            Users user = await _studentRepository.GetStudentById(id);
            if (file != null)
            {
                string imageUrl = await _cloudinaryService.UploadFileAsync(file);
                Post post = new Post();
                post.post = imageUrl;
                post.title = file.FileName;
                post.created = "c";
                post.id = user.posts.Count.ToString();
                Console.WriteLine(id);
                user.posts.Add(post);
                await _studentRepository.UpdateStudent(user);
            }
        }
        public async Task<string> Login(string name, string password)
        {
            return await _studentRepository.Login(name, password);
        }
        /*
        public List<string> GetStudentNamesStartsWithA()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < students.Count; i++)
            {
                if (students[i].StartsWith("A"))
                    list.Add(students[i]);
            }
            return list;
        }

        public List<string> GetStudentNamesOtherThanA()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < students.Count; i++)
            {
                if (students[i].StartsWith("A") == false)
                    list.Add(students[i]);
            }
            return list;
        }
              
        public List<string> GetStudentNamesWithA()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < students.Count; i++)
            {
                if (students[i].Contains('a') || students[i].Contains('A'))
                    list.Add(students[i]);
            }
            return list;
        }

        public string GetStudentById(int id)
        {
            if(id >= 1 && id <= students.Count)
                return students[id - 1];
            return "-1";
        }
        */
    }
}
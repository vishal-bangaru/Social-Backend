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
        public Task Insert(Users student);
        public Task Update(Users student);
        public Task Delete(string id);
        public Task PostImg(IFormFile file, string id);
        public Task<string> Login(string name, string password);
        /*
        public List<string> GetStudentNamesStartsWithA();

        public List<string> GetStudentNamesOtherThanA();

        public List<string> GetStudentNamesWithA();

        public string GetStudentById(int id);
        */
    }
}
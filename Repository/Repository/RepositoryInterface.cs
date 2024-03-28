using Common.Models;
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
        public Task<Users> GetStudentById(string studentId);
        public Task InsertStudent(Users student);
        public Task UpdateStudent(Users student);
        public Task DeleteStudent(string id);
        public Task<string> Login(string name, string password);
    }
}
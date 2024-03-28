
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Http;
using PBLClass.Service;
using Common.Models;
using Common.Services;

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
            await _studentInterface.Delete(student.id);
        }

        [HttpPost]
        [Route("PostImg")]
        public async Task PostImg() // file, userid
        {
            try
            {
                var file = Request.Form.Files["file"];
                if (file == null) return;
                var id = Request.Form["user"];
                await _studentInterface.PostImg(file, id);
            }
            catch (Exception ex)
            {
                return;
            }
        }
        [HttpPost]
        [Route("Login")]
        public async Task<string> Login(Users student)
        {
            return await _studentInterface.Login(student.name, student.password);
        }
        /*
        [HttpGet]
        [Route("GetStudentsStartsWith'A'")]
        public List<string> GetStudentsStartsWithA()
        {
            return _studentInterface.GetStudentNamesStartsWithA();
        }

        [HttpGet]
        [Route("GetStudentsOtherThan'A'")]
        public List<string> GetStudentsOtherThanA()
        {
            return _studentInterface.GetStudentNamesOtherThanA();
        }

        [HttpGet]
        [Route("GetStudentsWith'A'")]
        public List<string> GetStudentsWithA()
        {
            return _studentInterface.GetStudentNamesWithA();
        }

        [HttpGet]
        [Route("GetStudentById/{id}")]
        public ActionResult<string> Get(int id)
        {
            string std = _studentInterface.GetStudentById(id);
            if( std == "-1") return NotFound();
            return Ok(std);
        }
        */
    }
}
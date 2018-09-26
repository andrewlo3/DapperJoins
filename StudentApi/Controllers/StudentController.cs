using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using StudentApi.Models;

namespace StudentApi.Controllers
{
    [Route("api/stu")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentContext _context;

        public StudentController(StudentContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Student>> GetAll()
        {
            List<Student> stu = _context.GetAllStudents();
            if (stu == null)
                return NotFound();
            return stu;
        }

        [HttpGet("{id}", Name = "GetStudent")]
        public ActionResult<Student> GetById(string id)
        {
            Student stu = _context.GetStudent(id);
            if (stu == null)
                return NotFound();
            return stu;
        }

        [HttpGet("range")]
        public ActionResult<List<float>> GetGpaRange()
        {
            List<float> range = _context.GetGpaRange();
            if (range == null)
                return NotFound();
            return range;
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            _context.AddStudent(student);

            return CreatedAtRoute("GetStudent", new { id = student.Id }, student);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            int rowsDeleted = _context.DeleteStudent(id);

            if (rowsDeleted == 0)
                return NotFound();

            return NoContent();
        }
    }
}
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
        public ActionResult<List<Student>> GetStudents()
        {
            List<Student> stu = _context.GetStudents();
            if (stu == null)
                return NotFound();
            return stu;
        }

        [HttpGet("inv")]
        public ActionResult<List<Invoice>> GetInvoices()
        {
            List<Invoice> invoices = _context.GetInvoices();
            if (invoices == null)
                return NotFound();
            return invoices;
        }

        [HttpGet("cusInv")]
        public ActionResult<List<Customer>> GetCustomers()
        {
            List<Customer> customers = _context.GetCustomers();
            if (customers == null)
                return NotFound();
            return customers;
        }

        [HttpGet("{id}", Name = "GetStudent")]
        public ActionResult<Student> GetStudentById(string id)
        {
            Student stu = _context.GetStudentById(id);
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
using JwtApi.Data;
using JwtApi.Models;
using JwtApi.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Student
        [HttpGet]
        public IActionResult GetStudents(int page = 1,
            int pageSize = 4,
            string searchString = "",
            string sortColumn = "Id",
            string sortOrder = "asc")
        {
            // Use queryable for efficient filtering/sorting before execution
            IQueryable<Student> query = _context.Students.AsNoTracking();

            // Search
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s =>
                    s.StudentName.Contains(searchString)    ||
                    s.Email.Contains(searchString)    ||
                    s.Course.Contains(searchString));
            }

            // sorting
            switch (sortColumn)
            {
                case "StudentName":
                    query = sortOrder == "asc" ?
                         query.OrderBy(s => s.StudentName) : query.OrderByDescending(s => s.StudentName);
                    break;

                case "Email":
                    query = sortOrder == "asc" ?
                         query.OrderBy(s => s.Email) : query.OrderByDescending(s => s.Email);
                    break;

                case "Course":
                    query = sortOrder == "asc" ?
                         query.OrderBy(s => s.Course) : query.OrderByDescending(s => s.Course);
                    break;

                case "EnrollmentDate":
                    query = sortOrder == "asc" ?
                         query.OrderBy(s => s.EnrollmentDate) : query.OrderByDescending(s => s.EnrollmentDate);
                    break;

                default:
                    query = sortOrder == "asc" ?
                         query.OrderBy(s => s.Id) : query.OrderByDescending(s => s.Id);
                    break;

            }

            //************** Pagination 
            int totalCount = query.Count();
            var students = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new PaginatedList<Student>(students, totalCount, page, pageSize, sortColumn, sortOrder);

            return Ok(model);
        }

        // GET:************* api/Student/2
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {

            var student = _context.Students.Find(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        // POST:************** api/Student
        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Students.Add(student);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);      
        }

        // PUT:*************** api/Student/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, Student student)
        {
            if (id != student.Id) return BadRequest("ID Mismatch");

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine("error");
            }

            return NoContent();
        }

        // DELETE:*************** api/Student/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            _context.SaveChanges();

            return Ok(new { message = "Student deleted successfully" });
        }
    }
}

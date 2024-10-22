using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment7.Data;
using Assignment7.Models;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace Assignment7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly Assignment7Context _context;

        public StudentsController(Assignment7Context context)
        {
            _context = context;

            if (_context.Course.Count() == 0)
            {
                _context.Course.Add(new Course
                {
                    CourseID = 1,
                    Title = "Internet Programming 2",
                    Credits = 3
                });
                _context.Course.Add(new Course
                {
                    CourseID = 2,
                    Title = "Data Communications",
                    Credits = 3
                });
                _context.SaveChanges();
            }

            if (_context.Student.Count() == 0)
            {
                _context.Student.Add(new Student
                {
                    StudentID = 1,
                    LastName = "Sutariya",
                    FirstMidName = "Jayam Vikram",
                    EnrollmentDate = System.DateTime.Today
                });
                _context.Student.Add(new Student
                {
                    StudentID = 2,
                    LastName = "Cox",
                    FirstMidName = "Wesley",
                    EnrollmentDate = System.DateTime.Today
                });
                _context.SaveChanges();
            }
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
        {
            return await _context.Student.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Student.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.StudentID)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Student.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.StudentID }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Student.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Students/Register
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<Student>> RegisterStudent([FromBody] Register data)
        {
            var student = _context.Student.Find(data.StudentID);
            var course = _context.Course.SingleOrDefault(c => c.CourseID == data.CourseID);

            if (student == null || course == null)
            {
                return BadRequest("Invalid StudentID or CourseID");
            }

            if (course.CourseID == 0 || string.IsNullOrEmpty(course.Title) || course.Credits == 0)
            {
                return BadRequest("Invalid course data");
            }

            if (student.Courses == null)
            {
                student.Courses = new List<Course>();
            }

            student.Courses.Add(course);

            Console.WriteLine("Courses:");
            foreach (var c in student.Courses)
            {
                Console.WriteLine("Course ID: {0}, Title: {1}, Credits: {2}", c.CourseID, c.Title, c.Credits);
            }
            
            _context.Entry(student).State = !student.Courses.Any() ? EntityState.Added : EntityState.Modified;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.StudentID }, student);
        }

        [HttpGet("CourseRegistration/{id}")]
        public async Task<ActionResult<Student>> GetRegistered(int id)
        {
            var course = _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var registers = _context.Student
                .Where(s => s.Courses.Any(c => c.CourseID == id))
                .ToList();

            return Ok(registers);
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.StudentID == id);
        }
    }
}

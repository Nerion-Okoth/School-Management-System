using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementBackend.Data;
using SchoolManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagementBackend.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public ClassController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/Class
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetClasses()
        {
            return await _context.Classes
                .Include(c => c.Teacher)
                .ToListAsync();
        }

        // GET: api/Class/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> GetClass(int id)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classEntity == null)
            {
                return NotFound();
            }

            return classEntity;
        }

        // POST: api/Class
        [HttpPost]
        public async Task<ActionResult<Class>> CreateClass(Class request)
        {
            // Check if a teacher is associated, or create a default one
            var teacher = await _context.Teachers.FindAsync(request.TeacherId);

            if (teacher == null)
            {
                teacher = new Teacher
                {
                    FullName = "Default Teacher",
                    Email = "defaultteacher@school.com",
                    PhoneNumber = "0000000000"
                    
                };

                _context.Teachers.Add(teacher);
                await _context.SaveChangesAsync();

                request.TeacherId = teacher.Id; // Associate the new teacher
            }

            // Create a new class
            var newClass = new Class
            {
                Name = request.Name,
                Description = request.Description,
                TeacherId = request.TeacherId,
                Teacher = teacher,
                Students = request.Students ?? new List<Student>()
            };

            _context.Classes.Add(newClass);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClass), new { id = newClass.Id }, newClass);
        }

        // PUT: api/Class/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass(int id, Class request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity == null)
            {
                return NotFound();
            }

            classEntity.Name = request.Name;
            classEntity.Description = request.Description;
            classEntity.TeacherId = request.TeacherId;

            _context.Entry(classEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(id))
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

        // DELETE: api/Class/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity == null)
            {
                return NotFound();
            }

            _context.Classes.Remove(classEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.Id == id);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementBackend.Data;
using SchoolManagementBackend.Dtos;
using SchoolManagementSystem.Models;


[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public TeachersController(SchoolDbContext context)
    {
        _context = context;
    }

    // GET: api/teachers
    [HttpGet]
    public async Task<IActionResult> GetAllTeachers()
    {
        try
        {
            var teachers = await _context.Teachers
                .Include(t => t.Classes) // Include related Classes
                .Include(t => t.Courses) // Include related Courses
                .ToListAsync();

            return Ok(teachers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching teachers.", Details = ex.Message });
        }
    }

    // GET: api/teachers/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeacherById(int id)
    {
        try
        {
            var teacher = await _context.Teachers
                .Include(t => t.Classes)
                .Include(t => t.Courses)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
            {
                return NotFound(new { Message = "Teacher not found." });
            }

            return Ok(teacher);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching the teacher.", Details = ex.Message });
        }
    }

    // POST: api/teachers
    [HttpPost]
    public async Task<IActionResult> AddTeacher([FromBody] TeacherDTO teacherDto)
    {
        if (teacherDto == null)
        {
            return BadRequest(new { Message = "Teacher data is required." });
        }

        try
        {
            // Fetch related classes
            var classes = await _context.Classes
                .Where(c => teacherDto.Classes.Contains(c.Id))
                .ToListAsync();

            // Fetch related courses
            var courses = await _context.Courses
                .Where(c => teacherDto.Courses.Contains(c.Id))
                .ToListAsync();

            var teacher = new Teacher
            {
                FullName = teacherDto.FullName,
                Email = teacherDto.Email,
                PhoneNumber = teacherDto.PhoneNumber,
                Classes = classes,
                Courses = courses
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeacherById), new { id = teacher.Id }, teacher);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while adding the teacher.", Details = ex.Message });
        }
    }

    // PUT: api/teachers/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeacher(int id, [FromBody] TeacherDTO teacherDto)
    {
        if (teacherDto == null)
        {
            return BadRequest(new { Message = "Teacher data is required." });
        }

        try
        {
            var teacher = await _context.Teachers
                .Include(t => t.Classes)
                .Include(t => t.Courses)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
            {
                return NotFound(new { Message = "Teacher not found." });
            }

            // Fetch related classes
            var classes = await _context.Classes
                .Where(c => teacherDto.Classes.Contains(c.Id))
                .ToListAsync();

            // Fetch related courses
            var courses = await _context.Courses
                .Where(c => teacherDto.Courses.Contains(c.Id))
                .ToListAsync();

            teacher.FullName = teacherDto.FullName;
            teacher.Email = teacherDto.Email;
            teacher.PhoneNumber = teacherDto.PhoneNumber;
            teacher.Classes = classes;
            teacher.Courses = courses;

            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the teacher.", Details = ex.Message });
        }
    }

    // DELETE: api/teachers/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeacher(int id)
    {
        try
        {
            var teacher = await _context.Teachers
                .Include(t => t.Classes)
                .Include(t => t.Courses)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
            {
                return NotFound(new { Message = "Teacher not found." });
            }

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the teacher.", Details = ex.Message });
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementBackend.Data;
using SchoolManagementBackend.Dtos;
using SchoolManagementBackend.Models;
using SchoolManagementSystem.Models;


[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public StudentsController(SchoolDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await _context.Students
                                     .Include(s => s.Class)
                                     .Include(s => s.Course)
                                     .ToListAsync();
        return Ok(students);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudent(int id)
    {
        var student = await _context.Students
                                     .Include(s => s.Class)
                                     .Include(s => s.Course)
                                     .FirstOrDefaultAsync(s => s.Id == id);
        if (student == null)
            return NotFound(new { Message = "Student not found" });

        return Ok(student);
    }

    [HttpPost]
    public async Task<IActionResult> AddStudent(StudentDTO studentDto)
    {
        // Validate if the Class exists
        var classEntity = await _context.Classes.FindAsync(studentDto.ClassId);
        if (classEntity == null)
        {
            return BadRequest(new { Message = "Invalid ClassId. Class does not exist." });
        }

        // Validate if the Course exists
        var courseEntity = await _context.Courses.FindAsync(studentDto.CourseId);
        if (courseEntity == null)
        {
            return BadRequest(new { Message = "Invalid CourseId. Course does not exist." });
        }

        var student = new Student
        {
            FirstName = studentDto.FirstName,
            LastName = studentDto.LastName,
            ClassId = studentDto.ClassId, // Assign the foreign key directly
            CourseId = studentDto.CourseId, // Assign the foreign key directly
            Age = studentDto.Age
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, StudentDTO studentDto)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return NotFound(new { Message = "Student not found" });

        // Validate if the Class exists
        var classEntity = await _context.Classes.FindAsync(studentDto.ClassId);
        if (classEntity == null)
        {
            return BadRequest(new { Message = "Invalid ClassId. Class does not exist." });
        }

        // Validate if the Course exists
        var courseEntity = await _context.Courses.FindAsync(studentDto.CourseId);
        if (courseEntity == null)
        {
            return BadRequest(new { Message = "Invalid CourseId. Course does not exist." });
        }

        // Update properties
        student.FirstName = studentDto.FirstName;
        student.LastName = studentDto.LastName;
        student.Age = studentDto.Age;
        student.ClassId = studentDto.ClassId;
        student.CourseId = studentDto.CourseId;

        await _context.SaveChangesAsync();
        return Ok(student);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return NotFound(new { Message = "Student not found" });

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

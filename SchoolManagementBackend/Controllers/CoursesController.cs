using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementBackend.Data;
using SchoolManagementBackend.Dtos;
using SchoolManagementBackend.Models;
using SchoolManagementSystem.Models;


[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public CoursesController(SchoolDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        var courses = await _context.Courses.ToListAsync();
        return Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourse(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
            return NotFound(new { Message = "Course not found" });

        return Ok(course);
    }

    [HttpPost]
    public async Task<IActionResult> AddCourse(CourseDTO courseDto)
    {
        var course = new Course
        {
            Name = courseDto.Name,
            Description = courseDto.Description,
            CreditHours = courseDto.CreditHours,
            Grades = new List<Grade>() // Initialize Grades property
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id, CourseDTO courseDto)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
            return NotFound(new { Message = "Course not found" });

        course.Name = courseDto.Name;
        course.Description = courseDto.Description;
        course.CreditHours = courseDto.CreditHours;

        await _context.SaveChangesAsync();
        return Ok(course);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
            return NotFound(new { Message = "Course not found" });

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

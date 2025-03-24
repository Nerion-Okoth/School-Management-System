using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementBackend.Data;
using SchoolManagementBackend.Dtos;
using SchoolManagementSystem.Models;


[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public GradesController(SchoolDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGrades()
    {
        var grades = await _context.Grades.Include(g => g.Student).Include(g => g.Course).ToListAsync();
        return Ok(grades);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGrade(int id)
    {
        var grade = await _context.Grades.Include(g => g.Student).Include(g => g.Course)
                         .FirstOrDefaultAsync(g => g.Id == id);
        if (grade == null)
            return NotFound(new { Message = "Grade not found" });

        return Ok(grade);
    }

  [HttpPost]
public async Task<IActionResult> AddGrade(GradeDTO gradeDto)
{
    // Check if the Student and Course exist
    var student = await _context.Students.FindAsync(gradeDto.StudentId);
    if (student == null)
        return NotFound(new { Message = "Student not found" });

    var course = await _context.Courses.FindAsync(gradeDto.CourseId);
    if (course == null)
        return NotFound(new { Message = "Course not found" });

    // Create the Grade
    var grade = new Grade
    {
        StudentId = gradeDto.StudentId,
        CourseId = gradeDto.CourseId,
        Score = gradeDto.Score,
        Student = student,
        Course = course
    };

    // Add to the database and save
    _context.Grades.Add(grade);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetGrade), new { id = grade.Id }, grade);
}


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGrade(int id, GradeDTO gradeDto)
    {
        var grade = await _context.Grades.FindAsync(id);
        if (grade == null)
            return NotFound(new { Message = "Grade not found" });

        grade.Score = gradeDto.Score;

        await _context.SaveChangesAsync();
        return Ok(grade);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGrade(int id)
    {
        var grade = await _context.Grades.FindAsync(id);
        if (grade == null)
            return NotFound(new { Message = "Grade not found" });

        _context.Grades.Remove(grade);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementBackend.Data;
using SchoolManagementBackend.Dtos;
using SchoolManagementBackend.Models;
using SchoolManagementSystem.Models;


[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public AttendanceController(SchoolDbContext context)
    {
        _context = context;
    }

    // GET: api/attendance
    [HttpGet]
    public async Task<IActionResult> GetAllAttendance()
    {
        var attendances = await _context.Attendances
            .Include(a => a.Student)
            .Include(a => a.Course)
            .ToListAsync();

        return Ok(attendances);
    }

    // GET: api/attendance/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAttendanceById(int id)
    {
        var attendance = await _context.Attendances
            .Include(a => a.Student)
            .Include(a => a.Course)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (attendance == null)
        {
            return NotFound("Attendance record not found.");
        }

        return Ok(attendance);
    }

    // POST: api/attendance
    [HttpPost]
    public async Task<IActionResult> MarkAttendance([FromBody] AttendanceDTO attendanceDto)
    {
        if (attendanceDto == null)
        {
            return BadRequest("Attendance data is null.");
        }

        var student = await _context.Students.FindAsync(attendanceDto.StudentId);
        if (student == null)
        {
            return NotFound("Student not found.");
        }

        var course = await _context.Courses.FindAsync(attendanceDto.CourseId);
        if (course == null)
        {
            return BadRequest("Invalid CourseId. The specified course does not exist.");
        }

        var attendance = new Attendance
        {
            StudentId = attendanceDto.StudentId,
            Date = attendanceDto.Date,
            IsPresent = attendanceDto.IsPresent,
            Student = student,
            Course = course
        };

        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAttendanceById), new { id = attendance.Id }, attendance);
    }

    // PUT: api/attendance/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAttendance(int id, [FromBody] AttendanceDTO attendanceDto)
    {
        if (attendanceDto == null)
        {
            return BadRequest("Attendance data is null.");
        }

        var attendance = await _context.Attendances
            .Include(a => a.Student)
            .Include(a => a.Course)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (attendance == null)
        {
            return NotFound("Attendance record not found.");
        }

        var student = await _context.Students.FindAsync(attendanceDto.StudentId);
        if (student == null)
        {
            return NotFound("Student not found.");
        }

        var course = await _context.Courses.FindAsync(attendanceDto.CourseId);
        if (course == null)
        {
            return BadRequest("Invalid CourseId. The specified course does not exist.");
        }

        attendance.StudentId = attendanceDto.StudentId;
        attendance.Date = attendanceDto.Date;
        attendance.IsPresent = attendanceDto.IsPresent;
        attendance.Student = student;
        attendance.Course = course;

        _context.Attendances.Update(attendance);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/attendance/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttendance(int id)
    {
        var attendance = await _context.Attendances.FindAsync(id);
        if (attendance == null)
        {
            return NotFound("Attendance record not found.");
        }

        _context.Attendances.Remove(attendance);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementBackend.Data;
using SchoolManagementBackend.Dtos;
using SchoolManagementBackend.Models;
using SchoolManagementSystem.Models;


[ApiController]
[Route("api/[controller]")]
public class ParentsController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public ParentsController(SchoolDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllParents()
    {
        var parents = await _context.Parents.Include(p => p.Students).ToListAsync();
        return Ok(parents);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetParent(int id)
    {
        var parent = await _context.Parents.Include(p => p.Students)
                        .FirstOrDefaultAsync(p => p.Id == id);
        if (parent == null)
            return NotFound(new { Message = "Parent not found" });

        return Ok(parent);
    }

    [HttpPost]
    public async Task<IActionResult> AddParent(ParentDTO parentDto)
    {
        var parent = new Parent
        {
            FullName = parentDto.FullName,
            PhoneNumber = parentDto.PhoneNumber,
            Email = parentDto.Email,
            Relationships = new List<Relationship>() // Initialize the Relationships property
        };

        _context.Parents.Add(parent);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetParent), new { id = parent.Id }, parent);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateParent(int id, ParentDTO parentDto)
    {
        var parent = await _context.Parents.FindAsync(id);
        if (parent == null)
            return NotFound(new { Message = "Parent not found" });

        parent.FullName = parentDto.FullName;
        parent.PhoneNumber = parentDto.PhoneNumber;
        parent.Email = parentDto.Email;

        await _context.SaveChangesAsync();
        return Ok(parent);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteParent(int id)
    {
        var parent = await _context.Parents.FindAsync(id);
        if (parent == null)
            return NotFound(new { Message = "Parent not found" });

        _context.Parents.Remove(parent);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

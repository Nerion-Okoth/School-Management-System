using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolManagementBackend.Models;

namespace SchoolManagementSystem.Models
{
public class Parent
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }

    // Navigation property for Students
    public ICollection<Student> Students { get; set; } = new List<Student>();
        public List<Relationship>? Relationships { get; internal set; }
    }



}

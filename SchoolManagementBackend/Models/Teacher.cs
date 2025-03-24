using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models
{
    public class Teacher
    {
        // Primary Key
        public int Id { get; set; }

        // Teacher's Full Name
        [Required]
        [MaxLength(100)] // Optional length limit
        public required string FullName { get; set; }

        // Teacher's Email Address
        [Required]
        [EmailAddress] // Ensures valid email format
        public required string Email { get; set; }

        // Teacher's Phone Number
        [Required]
        [Phone] // Ensures valid phone number format
        public required string PhoneNumber { get; set; }

        // Navigation property for Courses taught by the Teacher (one-to-many)
        public ICollection<Course> Courses { get; set; } = new List<Course>();

        // Navigation property for Classes assigned to the Teacher (one-to-many)
        public ICollection<Class> Classes { get; set; } = new List<Class>();
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SchoolManagementBackend.Models;

namespace SchoolManagementSystem.Models
{
    public class Student : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)] // Limit the length of the first name
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)] // Limit the length of the last name
        public string LastName { get; set; } = string.Empty;

        [Required]
        public int ClassId { get; set; } // Foreign Key for SchoolClass

        // Navigation property for SchoolClass
        public Class Class { get; set; } = null!;

        // Optional property for Age
        public int? Age { get; set; }

        [Required]
        public int CourseId { get; set; } // Foreign Key for Course

        // Navigation property for Course
        public Course Course { get; set; } = null!;

        // Navigation property for Grades (one-to-many relationship)
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();

        // Navigation property for Relationships (many-to-many relationship with Parents)
        public ICollection<SchoolManagementBackend.Models.Relationship> Relationships { get; set; } = new List<SchoolManagementBackend.Models.Relationship>();

        // Navigation property for Attendance records
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }

    public class SchoolClass
    {
        public int Id { get; set; } // Corrected Class ID property name

        [Required]
        [MaxLength(100)] // Limit the length of the class name
        public string Name { get; set; } = string.Empty;

        [Required]
        public int TeacherId { get; set; } // Foreign Key for Teacher

        // Navigation property for Teacher
        public Teacher Teacher { get; set; } = null!;

        // Navigation property for Students in the Class
        public ICollection<Student> Students { get; set; } = new List<Student>();

        // Optional description for the class
        public string? Description { get; set; }
    }
}

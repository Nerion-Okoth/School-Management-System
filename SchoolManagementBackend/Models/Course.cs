using System.Collections.Generic;

namespace SchoolManagementSystem.Models
{
    public class Course
    {
        // Primary Key
        public int Id { get; set; }

        // Course Name
        public required string Name { get; set; }

        // Course Description
        public required string Description { get; set; }

        // Credit Hours for the course
        public int CreditHours { get; set; }

        // Foreign Key for Teacher
        public int? TeacherId { get; set; } // Allow null if a course can exist without a teacher

        // Navigation property for the Teacher who teaches the course
        public Teacher? Teacher { get; set; } // Nullable to avoid EF Core conflicts if not always assigned

        // Navigation property for Students enrolled in the course
        public ICollection<Student> Students { get; set; } = new List<Student>();

        // Navigation property for Grades associated with the course
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();

        // Navigation property for Attendances associated with the course
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}

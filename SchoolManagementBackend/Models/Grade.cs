using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models
{
    public class Grade
    {
        public int Id { get; set; }

        // Foreign Key for Student
        public int StudentId { get; set; }
        public Student? Student { get; set; } // Make non-nullable to ensure consistency

        // Foreign Key for Course
        public int CourseId { get; set; }
        public Course? Course { get; set; } // Make non-nullable to ensure consistency

        // The score for the grade
        [Range(0, 100, ErrorMessage = "Score must be between 0 and 100")]
        public double Score { get; set; }
    }
}

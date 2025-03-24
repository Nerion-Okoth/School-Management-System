using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        // Foreign key for Student
        public int StudentId { get; set; }

        // Foreign key for Course
        public int CourseId { get; set; }

        // Date of the attendance
        public DateTime Date { get; set; }

        // Indicates whether the student was present or not
        public bool IsPresent { get; set; }

        // Navigation Properties
        public Student? Student { get; set; }
        public Course? Course { get; set; }
    }
}

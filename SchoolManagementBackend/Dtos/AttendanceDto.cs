using System;
using SchoolManagementSystem.Models;

namespace SchoolManagementBackend.Dtos
{
    public class AttendanceDTO
    {
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }

        // Foreign Key for Course
        public int CourseId { get; set; }

        // Optional: Include the name of the course for better context
        public string CourseName { get; set; } = string.Empty;
        public Course? Course { get; internal set; }
    }
}

using System;

namespace SchoolManagementBackend.Dtos;

public class GradeDTO
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public double Score { get; set; } // Match type with Grade.Score
}

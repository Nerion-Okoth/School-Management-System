using System;

namespace SchoolManagementBackend.Dtos;

public class ClassDTO
{
    public required string Name { get; set; }
    public required string TeacherName { get; set; }
    public required List<StudentDTO> Students { get; set; }
}

public class StudentperfomanceDTO
{
    public int StudentId { get; set; }
    public required string StudentName { get; set; }
    public double AverageScore { get; set; }
}

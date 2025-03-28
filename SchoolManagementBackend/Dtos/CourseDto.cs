using System;

namespace SchoolManagementBackend.Dtos;

public class CourseDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int CreditHours { get; set; }
}

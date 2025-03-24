using System;

namespace SchoolManagementBackend.Dtos;

public class StudentDTO
{
    public StudentDTO()
    {
    }

    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int Age { get; set; }
    public int CourseId { get; set; }
    public int ClassId { get; set; }

    
}

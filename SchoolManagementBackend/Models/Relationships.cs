using System;
using SchoolManagementSystem.Models;

namespace SchoolManagementBackend.Models;

public class Relationship
{
    public int StudentId { get; set; }
    public int ParentId { get; set; }

    public Student? Student { get; set; }
    public Parent? Parent { get; set; }
}


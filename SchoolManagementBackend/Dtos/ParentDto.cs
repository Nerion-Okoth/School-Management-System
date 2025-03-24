using System;

namespace SchoolManagementBackend.Dtos;

public class ParentDTO
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
}

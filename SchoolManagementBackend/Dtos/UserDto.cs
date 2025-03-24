using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementBackend.Dtos;

public class UserDto
{
    public string UserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Username is required.")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public required string Password { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = "Student"; // Default role
}

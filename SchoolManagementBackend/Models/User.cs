using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementBackend.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Username { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [MaxLength(255)]
    public required string PasswordHash { get; set; }

    [Required]
    [MaxLength(20)]
    public required string Role { get; set; } = "Student"; // Default role if not specified

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property for Profile
    public Profile? Profile { get; set; } // Optional for login and can be null
}

using System.ComponentModel.DataAnnotations;

namespace SchoolManagementBackend.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "Username or email is required.")]
    public required string Identifier { get; set; } // Can be username or email

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public required string Password { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementBackend.Models
{
    public class Profile
    {
        // Parameterless constructor required by Entity Framework
        public Profile() { }

        public Profile(User adminUser)
        {
            AdminUser = adminUser;
        }

        [Key]
        public int ProfileId { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; } // Foreign key to the User entity

        // Navigation property for the related User entity
        public User User { get; set; } = null!; // Non-nullable as EF will initialize it

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // Default value to avoid null warnings

        [Required]
        [MaxLength(50)]
        public string Contact { get; set; } = string.Empty; // Limited to a reasonable length

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [MaxLength(255)]
        public string Address { get; set; } = string.Empty; // Ensure a max length for the address field

        [Required]
        [MaxLength(2083)] // Maximum URL length for most browsers
        public string ImageUrl { get; set; } = string.Empty; // Path to profile image

        // Optional property for AdminUser (if needed)
        public User? AdminUser { get; set; } // Set as nullable to avoid initialization issues
    }
}

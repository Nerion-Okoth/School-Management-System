using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class Class
    {
        public Class()
        {
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public required string Description { get; set; }

        // Foreign key for the Teacher entity
        [ForeignKey("Teacher")]
        public int? TeacherId { get; set; }
        
        public Teacher ?Teacher { get; set; } // Navigation property for Teacher

        // Navigation property for associated students
        public List<Student> Students { get; set; } = new List<Student>();
    }
}

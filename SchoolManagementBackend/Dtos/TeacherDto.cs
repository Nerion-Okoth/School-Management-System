using System.Collections.Generic;

namespace SchoolManagementBackend.Dtos
{
    public class TeacherDTO
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }

        // A collection of Class IDs
        public ICollection<int> Classes { get; set; } = new List<int>();

        // A collection of Course IDs
        public ICollection<int> Courses { get; set; } = new List<int>();
    }
}

using System.ComponentModel.DataAnnotations;

namespace StudentLibraryManagement.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Department { get; set; } 
        public int Age { get; set; }
        public string? Gender { get; set; } 
        [EmailAddress]
        public string Email { get; set; }
    }
}

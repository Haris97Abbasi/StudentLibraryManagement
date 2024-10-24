using System.ComponentModel.DataAnnotations.Schema;

namespace StudentLibraryManagement.Models
{
    public class Books
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Shelf { get; set; }

        // Custom foreign key property name
        public int CategoryId { get; set; }
        public int StudentId { get; set; }

        // Navigation properties
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        
    }
}

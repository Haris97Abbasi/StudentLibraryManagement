using System.ComponentModel.DataAnnotations.Schema;

namespace StudentLibraryManagement.Models.ViewModels
{
    public class BooksViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int CategoryId { get; set; }
        public int StudentId { get; set; }
        public string Shelf { get; set; }
        public string CategoryName { get; set; }
        public string StudentName { get; set; }
    }
}

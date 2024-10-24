using Microsoft.AspNetCore.Mvc.Rendering;
using StudentLibraryManagement.Models;
using StudentLibraryManagement.Models.ViewModels;

namespace StudentLibraryManagement.Services
{
    public interface IBooksService
    {
        public List<BooksViewModel> GetBooks();
        public List<BooksViewModel> GetFilteredBooks(string searchTitle, string searchAuthor, string searchCategory, string searchShelf, string searchBorrower);
        public int AddBooks(Books book);
        public Books GetBookById(int id);
        public int UpdateBooks(Books book);
        public int DeleteBooks(int id);
        public List<Student> DisplayStudents();
        public IEnumerable<SelectListItem> StudentsDropDown();
        public List<Category> DisplayCategories();
        public IEnumerable<SelectListItem> CategoriesDropDown();
    }
}

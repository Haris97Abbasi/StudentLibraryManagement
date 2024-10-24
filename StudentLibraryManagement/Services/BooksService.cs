using Microsoft.EntityFrameworkCore;
using StudentLibraryManagement.Models;
using StudentLibraryManagement.Models.ViewModels;
using StudentLibraryManagement.Data;
using StudentLibraryManagement.Utility;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentLibraryManagement.Services
{
    
    public class BooksService : IBooksService
    {
        private readonly ApplicationDbContext _db;
        public BooksService(ApplicationDbContext db)
        {
            _db = db;
        }
        public List<BooksViewModel> GetBooks()
        {

            var BookList = (from book in _db.Books
                            select new BooksViewModel
                            {
                                Id = book.Id,
                                Title = book.Title,
                                Author = book.Author,
                                CategoryId = book.CategoryId,
                                StudentId = book.StudentId,
                                Shelf = book.Shelf,
                                CategoryName = (_db.Categories.FirstOrDefault(x => x.Id == book.CategoryId)).Name,
                                StudentName = (_db.Students.FirstOrDefault(x => x.Id == book.StudentId)).Name
                            }).ToList();
            return BookList;
        }


        public List<BooksViewModel> GetFilteredBooks(string searchTitle, string searchAuthor, string searchCategory, string searchShelf, string searchBorrower)
        {
            var query = from book in _db.Books
                        select new BooksViewModel
                        {
                            Id = book.Id,
                            Title = book.Title,
                            Author = book.Author,
                            CategoryId = book.CategoryId,
                            StudentId = book.StudentId,
                            Shelf = book.Shelf,
                            CategoryName = (_db.Categories.FirstOrDefault(x => x.Id == book.CategoryId)).Name,
                            StudentName = (_db.Students.FirstOrDefault(x => x.Id == book.StudentId)).Name
                        };

            // Apply search filters
            if (!string.IsNullOrEmpty(searchTitle))
            {
                query = query.Where(b => b.Title.Contains(searchTitle));
            }

            if (!string.IsNullOrEmpty(searchAuthor))
            {
                query = query.Where(b => b.Author.Contains(searchAuthor));
            }

            if (!string.IsNullOrEmpty(searchCategory))
            {
                query = query.Where(b => b.CategoryName.Contains(searchCategory));
            }

            if (!string.IsNullOrEmpty(searchShelf))
            {
                query = query.Where(b => b.Shelf.Contains(searchShelf));
            }

            if (!string.IsNullOrEmpty(searchBorrower))
            {
                query = query.Where(b => b.StudentName.Contains(searchBorrower));
            }

            return query.ToList();
        }


        public int AddBooks(Books book)
        {            
            _db.Books.Add(book);
            _db.SaveChanges();
            return 1;
            
        }

        public int UpdateBooks(Books book)
        {
            var editedBook = _db.Books.FirstOrDefault(x=> x.Id == book.Id);
            if (editedBook != null)
            {
                editedBook.Title = book.Title;
                editedBook.Author = book.Author;
                editedBook.CategoryId = book.CategoryId;
                editedBook.Shelf = book.Shelf;
                editedBook.StudentId = book.StudentId;
                _db.SaveChanges();
            }
            return 1;
        }

        public Books GetBookById(int id)
        {
            var book = _db.Books.FirstOrDefault(x => x.Id == id);
            return book;
        }
        public int DeleteBooks(int id)
        {
            var book  = _db.Books.FirstOrDefault(x=>x.Id ==  id);
            if (book != null)
            {
                _db.Books.Remove(book);
                _db.SaveChanges();
            }
            return 1;
        }

        public List<Student> DisplayStudents()
        {
            return _db.Students.ToList();
        }

        public IEnumerable<SelectListItem> StudentsDropDown()
        {
            IEnumerable<SelectListItem> TypeDropDown = _db.Students.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return TypeDropDown;
        }

        public List<Category> DisplayCategories()
        {
            return _db.Categories.ToList();
        }

        public IEnumerable<SelectListItem> CategoriesDropDown()
        {
            IEnumerable<SelectListItem> TypeDropDown = _db.Categories.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return TypeDropDown;
        }
    }
}

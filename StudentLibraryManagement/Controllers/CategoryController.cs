using Microsoft.AspNetCore.Mvc;
using StudentLibraryManagement.Services;

namespace StudentLibraryManagement.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IBooksService _booksService;
        public CategoryController(IBooksService booksService)
        {
            _booksService = booksService;
        }
        public IActionResult Index()
        {
            var categories = _booksService.DisplayCategories();
            return View(categories);
        }
    }
}

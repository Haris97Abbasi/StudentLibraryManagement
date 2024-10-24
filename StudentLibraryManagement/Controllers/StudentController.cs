using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentLibraryManagement.Services;

namespace StudentLibraryManagement.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IBooksService _booksService;
        public StudentController(IBooksService booksService)
        {
            _booksService = booksService;
        }
        public IActionResult Index()
        {
            var student = _booksService.DisplayStudents();
            return View(student);
        }
    }
}

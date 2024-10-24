using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentLibraryManagement.Models;
using StudentLibraryManagement.Services;
using OfficeOpenXml;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.IO;
using System;
using System.Runtime.InteropServices;

namespace StudentLibraryManagement.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly IBooksService _booksService;
        public BooksController(IBooksService booksService)
        {
            _booksService = booksService;
        }

        public IActionResult Index(string searchTitle, string searchAuthor, string searchCategory, string searchShelf, string searchBorrower)
        {
            // Pass search parameters to the service to get the filtered results
            var books = _booksService.GetFilteredBooks(searchTitle, searchAuthor, searchCategory, searchShelf, searchBorrower);
            return View(books);
        }


        public IActionResult AddBook()
        {
            ViewBag.TypeDropDown = _booksService.StudentsDropDown();
            ViewBag.CategoryDropDown = _booksService.CategoriesDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBook(Books book)
        {
            _booksService.AddBooks(book);
            return RedirectToAction("Index");
        }

        public IActionResult UpdateBook(int id)
        {
            ViewBag.TypeDropDown = _booksService.StudentsDropDown();
            ViewBag.CategoryDropDown = _booksService.CategoriesDropDown();
            var book = _booksService.GetBookById(id);
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PostUpdateBook(Books book)
        {
            _booksService.UpdateBooks(book);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteBook(int id)
        {
            _booksService.DeleteBooks(id);
            return RedirectToAction("Index");
        }

        public IActionResult DownloadBooks()
        {
            var books = _booksService.GetBooks(); // Get the list of books

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Books List");

                // Add Headers
                worksheet.Cells[1, 1].Value = "S.No.";
                worksheet.Cells[1, 2].Value = "Title";
                worksheet.Cells[1, 3].Value = "Author";
                worksheet.Cells[1, 4].Value = "Category";
                worksheet.Cells[1, 5].Value = "Shelf";
                worksheet.Cells[1, 6].Value = "Borrower";

                // Add Book Data
                int row = 2;
                foreach (var book in books)
                {
                    worksheet.Cells[row, 1].Value = book.Id;
                    worksheet.Cells[row, 2].Value = book.Title;
                    worksheet.Cells[row, 3].Value = book.Author;
                    worksheet.Cells[row, 4].Value = book.CategoryName;
                    worksheet.Cells[row, 5].Value = book.Shelf;
                    worksheet.Cells[row, 6].Value = book.StudentName;
                    row++;
                }

                // Set the column width to fit content
                worksheet.Cells.AutoFitColumns();

                // Create the file and download
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"BooksList-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        public IActionResult DownloadBooksPdf()
        {
            var books = _booksService.GetBooks(); // Get the list of books

            // Create a new PDF document
            PdfDocument pdf = new PdfDocument();
            PdfPage pdfPage = pdf.AddPage();
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
            XFont font = new XFont("Verdana", 12, XFontStyle.Regular);

            // Title
            graph.DrawString("Books List", new XFont("Verdana", 20, XFontStyle.Bold), XBrushes.Black, new XRect(0, 0, pdfPage.Width, 50), XStringFormats.Center);

            int yPoint = 80;
            foreach (var book in books)
            {
                string bookInfo = $"Id: {book.Id}, Title: {book.Title}, Author: {book.Author}, Category: {book.CategoryName}, Shelf: {book.Shelf}, Borrower: {book.StudentName}";
                graph.DrawString(bookInfo, font, XBrushes.Black, new XRect(40, yPoint, pdfPage.Width, pdfPage.Height), XStringFormats.TopLeft);
                yPoint += 25;
            }

            // Save the document into a memory stream
            using (MemoryStream stream = new MemoryStream())
            {
                pdf.Save(stream, false);
                stream.Position = 0;
                string pdfName = $"BooksList-{DateTime.Now:yyyyMMddHHmmss}.pdf";
                return File(stream.ToArray(), "application/pdf", pdfName);
            }
        }

        public IActionResult DownloadBooksPdf2()
        {
            var books = _booksService.GetBooks(); // Get the list of books

            // Create a new PDF document
            PdfDocument pdf = new PdfDocument();
            PdfPage pdfPage = pdf.AddPage();
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
            XFont font = new XFont("Verdana", 12, XFontStyle.Regular);

            // Title
            graph.DrawString("Books List", new XFont("Verdana", 20, XFontStyle.Bold), XBrushes.Black,
                             new XRect(0, 0, pdfPage.Width, 50), XStringFormats.Center);

            int yPoint = 80;
            int maxLineWidth = 500; // Set a maximum line width (adjust as necessary)

            foreach (var book in books)
            {
                string bookInfo = $"Id: {book.Id}, Title: {book.Title}, Author: {book.Author}, " +
                                  $"Category: {book.CategoryName}, Shelf: {book.Shelf}, Borrower: {book.StudentName}";

                // Wrap text if it exceeds the maximum width
                List<string> wrappedLines = WrapText(graph, bookInfo, font, maxLineWidth);

                foreach (string line in wrappedLines)
                {
                    graph.DrawString(line, font, XBrushes.Black, new XRect(40, yPoint, pdfPage.Width - 80, pdfPage.Height), XStringFormats.TopLeft);
                    yPoint += 25;
                }
            }

            // Save the document into a memory stream
            using (MemoryStream stream = new MemoryStream())
            {
                pdf.Save(stream, false);
                stream.Position = 0;
                string pdfName = $"BooksList-{DateTime.Now:yyyyMMddHHmmss}.pdf";
                return File(stream.ToArray(), "application/pdf", pdfName);
            }
        }

        // Function to wrap text based on line width
        public List<string> WrapText(XGraphics graph, string text, XFont font, int maxWidth)
        {
            List<string> wrappedLines = new List<string>();
            string[] words = text.Split(' ');
            string currentLine = "";

            foreach (string word in words)
            {
                string testLine = (currentLine.Length == 0) ? word : currentLine + " " + word;
                XSize size = graph.MeasureString(testLine, font);

                if (size.Width < maxWidth)
                {
                    currentLine = testLine;
                }
                else
                {
                    wrappedLines.Add(currentLine);
                    currentLine = word; // Start a new line with the current word
                }
            }

            if (currentLine.Length > 0)
            {
                wrappedLines.Add(currentLine); // Add the last line
            }

            return wrappedLines;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        //[HttpGet("/books")]
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet("/books/new")]
        public IActionResult CreateBookForm()
        {
            return View();
        }

        [HttpPost("/books/new")]
        public IActionResult CreateBook(string bookTitle, string authorName, int copies)
        {
            Book newBook = new Book(bookTitle);
            Author newAuthor = new Author(authorName);
            Copy newCopy = new Copy(copies);
            newBook.Save(newAuthor, newCopy);
            newBook.AddBookDetails(newAuthor, newCopy);
            return View("CreateBookForm");
        }
    }
}

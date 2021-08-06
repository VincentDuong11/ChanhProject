using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChanhProject.Models;

namespace ChanhProject.Controllers
{
    public class BookController : Controller
    {
        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(BookModel book, string author_ids)
        {
            populateAuthors(book, author_ids);
            BookModel.Upsert(book);
            return View();
        }

        private void populateAuthors(BookModel book, string author_ids)
        {
            book.Authors = new List<AuthorModel>();
            foreach (var id in author_ids.Split(","))
            {
                if (id.Length == 0) continue;
                book.Authors.Add(AuthorModel.Get(Int64.Parse(id)));
            }
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            return View(BookModel.Get(id));
        }

        [HttpPost]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (HttpContext.Request.Method == "GET")
            {
                return RedirectToAction("Index", "Book");
            }

            BookModel.Delete(id);
            return RedirectToAction("Index","Book");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChanhProject.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChanhProject.Controllers
{
    public class AuthorController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(AuthorModel author)
        {
            AuthorModel.Upsert(author);
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            return View(AuthorModel.Get(id));
        }

        [HttpPost]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (HttpContext.Request.Method == "GET")
            {
                return RedirectToAction("Index", "Author");
            }

            AuthorModel.Delete(id);
            return RedirectToAction("Index", "Author");
        }
    }
}

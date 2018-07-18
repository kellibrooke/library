using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Library.Controllers
{
    public class PatronsController : Controller
    {
        [HttpGet("/patrons")]
        public IActionResult ViewAllPatrons()
        {
            return View(Patron.GetAllPatrons());
        }

        [HttpGet("/patrons/new")]
        public IActionResult CreatePatronForm()
        {
            return View();
        }

        [HttpPost("/patrons/new")]
        public IActionResult CreatePatron(string name)
        {
            Patron newPatron = new Patron(name);
            newPatron.Save();
            return RedirectToAction("ViewAllPatrons");
        }

        [HttpGet("/patrons/{id}")]
        public IActionResult PatronHome(int id)
        {
            Patron newPatron = Patron.FindPatron(id);
            return View(newPatron);
        }
    }
}

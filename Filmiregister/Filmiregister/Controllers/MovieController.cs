using Filmiregister.DatabaseContext;
using Filmiregister.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Filmiregister.Controllers
{
    [Route("[controller]")]
    public class MovieController : Controller
    {
        private readonly MovieContext _context;
        public MovieController(MovieContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Index","Home");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Home");

            var movie = _context.Movies.FirstOrDefault(m=>m.MovieID == id);

            if (movie == null) return RedirectToAction("Index", "Home");

            return View(movie);
        }
    }
}

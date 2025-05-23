using Filmiregister.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Filmiregister.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieContext _context;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Models.Movie> movies;
            try
            {
                movies = _context.Movies.OrderByDescending(y => y.Rating).ToList();
            }
            catch
            {
                movies = new List<Models.Movie>();
            }

            return View(movies);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Home");

            var movie = _context.Movies.FirstOrDefault(m => m.ID == id);

            if (movie == null) return RedirectToAction("Index", "Home");

            return View(movie);
        }
    }
}

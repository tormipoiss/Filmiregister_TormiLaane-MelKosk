using Filmiregister.DatabaseContext;
using Filmiregister.ViewModels;
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

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.ID == id);
            if (movie == null) return RedirectToAction("Index", "Home");

            // Get comments directly from context
            var comments = await _context.Comments
                .Where(c => c.MovieID == movie.ID)
                .OrderByDescending(c => c.CreationDate)
                .ToListAsync();

            var viewModel = new MovieDetails
            {
                Movie = movie,
                Comments = comments
            };

            return View(viewModel);
        }
    }
}

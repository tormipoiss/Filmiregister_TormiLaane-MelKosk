using Filmiregister.DatabaseContext;
using Filmiregister.Models;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost("Favorite")]
        [Authorize]
        public async Task<IActionResult> Favorite([FromForm] int id, [FromForm] string username)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m=>m.ID == id);
            if (movie == null) return RedirectToAction("Index", "Movies");
            var user = await _context.Users.FirstOrDefaultAsync(u=>u.UserName == username);
            if (user == null) return RedirectToAction("Index", "Movies");
            if (user.ChosenMovies == null) user.ChosenMovies = new();
            if (user.ChosenMovies.Contains(id)) return RedirectToAction("Index", "Movies");
            user.ChosenMovies.Add(id);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Movies");
        }
        [HttpPost("Favorites")]
        [Authorize]
        public async Task<ActionResult> Favorites([FromForm] string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u=>u.UserName == username);
            if (user == null) return RedirectToAction("Index", "Home");
            List<Movie> movies = new();
            foreach(int movieId in user.ChosenMovies)
            {
                var movie = await _context.Movies.FirstOrDefaultAsync(m=>m.ID == movieId);
                if(movie != null) movies.Add(movie);
            }
            return View(movies);

        }

    }
}

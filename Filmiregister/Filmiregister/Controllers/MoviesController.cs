using Filmiregister.DatabaseContext;
using Filmiregister.Models;
using Filmiregister.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Xml.Linq;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Filmiregister.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MoviesController(MovieContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = false;
            if (userId != null)
            {
                var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null && user.IsAdmin)
                {
                    isAdmin = true; // Only true if user.IsAdmin is true!
                }
            }


            var viewModel = new MovieIndex
            {
                Movies = movies,
                IsAdmin = isAdmin
            };

            return View(viewModel);
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = false;
            if (userId != null)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null && user.IsAdmin)
                {
                    isAdmin = true; // Only true if user.IsAdmin is true!
                }
            }


            var viewModel = new MovieDetails
            {
                Movie = movie,
                Comments = comments,
                IsAdmin = isAdmin
            };

            return View(viewModel);
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            // Get current user ID
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            // Fetch the user from the database
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || !user.IsAdmin)
            {
                return Forbid(); // Not an admin
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Movie movie)
        {
            // Get current user ID
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            // Fetch the user from the database
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || !user.IsAdmin)
            {
                return Forbid(); // Not an admin
            }

            if (id != movie.ID)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(movie);

            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Movies.Any(m => m.ID == id))
                    return NotFound();
                else
                    throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(int Id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || !user.IsAdmin)
            {
                return Forbid(); // Not an admin
            }
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.ID == Id);
            if(movie == null) return NotFound();
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Movie movie)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || !user.IsAdmin)
            {
                return Forbid(); // Not an admin
            }

            if (!ModelState.IsValid)
                return View(movie);
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

using Filmiregister.DatabaseContext;
using Filmiregister.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Filmiregister.Controllers
{
    public class CommentsController : Controller
    {
        private readonly MovieContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public CommentsController(MovieContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddComment(int movieId)
        {
            // Get movie title for display (optional)
            var movie = _context.Movies.FirstOrDefault(m => m.ID == movieId);
            if (movie == null)
            {
                return RedirectToAction("Index", "Movies");
            }

            ViewBag.MovieID = movieId;
            ViewBag.MovieTitle = movie.Title;

            return View(new Comment { MovieID = movieId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CreationDate = DateTime.Now;
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Your comment has been added successfully!";
                return RedirectToAction("Details", "Movies", new { id = comment.MovieID });
            }

            // If validation fails, return to form with errors
            ViewBag.MovieID = comment.MovieID;
            var movie = _context.Movies.FirstOrDefault(m => m.ID == comment.MovieID);
            ViewBag.MovieTitle = movie?.Title;

            return View(comment);
        }
    }
}

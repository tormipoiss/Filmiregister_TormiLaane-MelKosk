using Filmiregister.DatabaseContext;
using Filmiregister.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Filmiregister.Controllers
{
    public class CommentsController : Controller
    {
        private readonly MovieContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(MovieContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int ID)
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

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.ID == ID);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Movies", new { id = comment.MovieID });
        }
    }
}

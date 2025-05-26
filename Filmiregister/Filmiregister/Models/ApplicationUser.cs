using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Filmiregister.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAdmin { get; set; }
        public List<int>? ChosenMovies { get; set; } = [];
    }
}

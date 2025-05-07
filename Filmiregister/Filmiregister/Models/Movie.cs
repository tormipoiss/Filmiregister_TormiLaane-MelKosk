using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Filmiregister.Models
{
    public class Movie
    {
        public int MovieID { get; set; }
        public string MovieTitle { get; set; }
        public string MovieDescription { get; set; }
        public string MovieImage { get; set; }
        public string MovieSecondImage { get; set; }
        public int MovieRating { get; set; }
        public List<string> Categories { get; set; }
        public DateOnly PublicationDate { get; set; }
        public string MovieLanguage { get; set; }
        public TimeOnly Duration { get; set; }
        //public List<Account> UsersWhoOwnMovie { get; set; }
        //public List<Comment> CommentsOnMovie { get; set; }
    }
}

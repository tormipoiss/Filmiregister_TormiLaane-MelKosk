using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Filmiregister.Models
{
    public class Film
    {
        public int FilmID { get; set; }
        public string FilmTitle { get; set; }
        public string FilmDescription { get; set; }
        public string FilmImage { get; set; }
        public string FilmSecondImage { get; set; }
        public int FilmRating { get; set; }
        public List<string> Categories { get; set; }
        public DateOnly PublicationDate { get; set; }
        public string MovieLanguage { get; set; }
        //public List<Account> UsersWhoOwnMovie { get; set; }
        //public List<Comment> CommentsOnMovie { get; set; }
    }
}

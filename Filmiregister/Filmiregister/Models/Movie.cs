using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Filmiregister.Models
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string SecondImage { get; set; }
        public float Rating { get; set; }
        public List<string> Categories { get; set; }
        public DateOnly PublicationDate { get; set; }
        public string Language { get; set; }
        public TimeSpan Duration { get; set; }
        //public List<Account>? AccountsWhoOwnMovie { get; set; }
        //public List<Comment>? CommentsOnMovie { get; set; }
    }
}

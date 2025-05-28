using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;

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

        // Helper property for form binding
        [NotMapped] // This won't be stored in the database
        public string CategoriesString
        {
            get => Categories != null ? string.Join(", ", Categories) : string.Empty;
            set => Categories = value?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                     .Select(c => c.Trim())
                                     .ToList() ?? new List<string>();
        }
    }
}

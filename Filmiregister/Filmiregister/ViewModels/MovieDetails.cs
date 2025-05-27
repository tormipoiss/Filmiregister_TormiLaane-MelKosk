using Filmiregister.Models;

namespace Filmiregister.ViewModels
{
    public class MovieDetails
    {
        public Movie Movie { get; set; }
        public List<Comment> Comments { get; set; }
    }
}

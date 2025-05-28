using Filmiregister.Models;

namespace Filmiregister.ViewModels
{
    public class MovieIndex
    {
        public List<Movie> Movies { get; set; } = [];
        public bool IsAdmin { get; set; }
    }
}

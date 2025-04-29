using Microsoft.EntityFrameworkCore;

namespace Filmiregister.DatabaseContext
{
    public class FilmContext : DbContext
    {
        public FilmContext(DbContextOptions<FilmContext> options) : base(options) { }
    }
}

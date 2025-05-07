using Microsoft.EntityFrameworkCore;

namespace Filmiregister.DatabaseContext
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options) { }
    }
}

using Filmiregister.Models;
using Microsoft.EntityFrameworkCore;

namespace Filmiregister.DatabaseContext
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options) { }
        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().ToTable("Movies");
        }

    }
}

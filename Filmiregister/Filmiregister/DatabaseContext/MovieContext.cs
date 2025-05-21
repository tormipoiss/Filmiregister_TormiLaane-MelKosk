using Filmiregister.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Filmiregister.DatabaseContext
{
    public class MovieContext : IdentityDbContext<ApplicationUser>
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options) { }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        //public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().ToTable("Movies");
            modelBuilder.Entity<Comment>().ToTable("Comments");
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Account>().ToTable("Accounts");
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<ApplicationUser>().Property(p => p.Id).HasColumnName("UserId");
        }

    }
}

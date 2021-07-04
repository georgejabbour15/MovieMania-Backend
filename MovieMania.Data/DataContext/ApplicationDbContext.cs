using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieMania.Infrastructure.Domains;

namespace MovieMania.Data.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<MovieCountry> MovieCountries { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasOne(r => r.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasOne(r => r.User)
                    .WithMany(u => u.Ratings)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasMany(m => m.Ratings)
                    .WithOne(r => r.Movie)
                    .HasForeignKey(m => m.MovieId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MovieGenre>().HasKey(mg => new { mg.GenreId,mg.MovieId });
            modelBuilder.Entity<MovieCountry>().HasKey(mc => new { mc.CountryId,mc.MovieId });
        }

    }
}


using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieBuff.Models;

namespace MovieBuff.Data
{
    public class MovieBuffContext : IdentityDbContext<ApplicationUser>
    {
        public MovieBuffContext (DbContextOptions<MovieBuffContext> options)
            : base(options)
        {
        }
        public DbSet<MovieBuff.Models.Film> Films { get; set; } = default!;
        public DbSet<MovieBuff.Models.Favorite> Favorites { get; set; } = default!;
        public DbSet<MovieBuff.Models.Rating> Ratings { get; set; } = default!;
        public DbSet<MovieBuff.Models.WatchLater> WatchLaters { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<WatchLater>()
                .HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserId);
        }
    }
}
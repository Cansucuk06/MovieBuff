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
        public DbSet<MovieBuff.Models.ApplicationUser> ApplicationUsers { get; set; } = default!;
        public DbSet<MovieBuff.Models.Favorite> Favorites { get; set; } = default!;
        public DbSet<MovieBuff.Models.Rating> Ratings { get; set; } = default!;
        public DbSet<MovieBuff.Models.WatchLater> WatchLaters { get; set; } = default!;
    }
}
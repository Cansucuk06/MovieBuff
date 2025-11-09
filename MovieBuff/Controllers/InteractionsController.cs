using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MovieBuff.Models;
using MovieBuff.Data;
using System.Security.Claims;

public class InteractionRequest
{
    public int MovieId { get; set;}
}

public class RatingRequest
{
    public int MovieId { get; set; }
    public int Rating { get; set; }
}

namespace MovieBuff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InteractionsController : ControllerBase
    {
        private readonly MovieBuffContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public InteractionsController(MovieBuffContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpPost("HandleFavorites")]
        public async Task<IActionResult> HandleFavorites(InteractionRequest request)
        {
            var userId = GetUserId();
            var inFavorites = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.FilmId == request.MovieId);

            if (inFavorites == null)
            {
                var Favorites = new Favorite
                {
                    UserId = userId,
                    FilmId = request.MovieId
                };

                _context.Favorites.Add(Favorites);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, action = "added", message = "Film favorilere eklendi." });

            }
            else
            {
                _context.Favorites.Remove(inFavorites);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, action = "removed", message = "Film favorilerden kaldırıldı." });
            }
        }

        [HttpPost("HandleWatchLaters")]
        public async Task<IActionResult> HandleWatchLaters(InteractionRequest request)
        {
            var userId = GetUserId();
            var inWatchLater = await _context.WatchLaters
                .FirstOrDefaultAsync(w => w.UserId == userId && w.FilmId == request.MovieId);

            if (inWatchLater == null)
            {
                var watchLater = new WatchLater
                {
                    UserId = userId,
                    FilmId = request.MovieId
                };

                _context.WatchLaters.Add(watchLater);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, action = "added", message = "Film daha sonra izle listesine eklendi." });
            }
            else
            {
                _context.WatchLaters.Remove(inWatchLater);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, action = "removed", message = "Film daha sonra izel listesinden kaldırıldı." });
            }
        }

        [HttpPost("RateMovies")]
        public async Task<IActionResult> RateMovies(RatingRequest request)
        {
            var userId = GetUserId();
            var inRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.FilmId == request.MovieId);

            if (inRating == null)
            {
                var rating = new Rating
                {
                    UserId = userId,
                    FilmId = request.MovieId
                };
                _context.Ratings.Add(rating);
                
            }
            else
            {
                inRating.Score = request.Rating;
                _context.Ratings.Update(inRating);

            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Filmi değerlendirdiniz." });
        }
    }
}

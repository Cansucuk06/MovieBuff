using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MovieBuff.Models;
using MovieBuff.Data;
using System.Security.Claims;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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
        [HttpGet("Favorites")]
        public async Task<IActionResult> GetFavorites()
        {
            var userId = GetUserId();
            var favoriteFilmIds = await _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.FilmId)
                .ToListAsync();
            return Ok(favoriteFilmIds);
        }
        [HttpGet("Favorites/{filmId}")]
        public async Task<IActionResult> IsFavorite(int filmId)
        {
            var userId = GetUserId();
            var isFavorite = await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.FilmId == filmId);

            if (isFavorite)
            {
                return Ok(new { isFavorite = true });
            }
            return NotFound(new { isFavorite = false });
           
        }

        [HttpPost("Favorites")]
        public async Task<IActionResult> AddFavorite([FromBody] InteractionRequest request)
        {
            var userId = GetUserId();
            var exists = await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.FilmId == request.MovieId);

            if (exists)
            {
                return Conflict(new { success = false, message = "Film zaten favorilerinizde." });
            }
            var favorite = new Favorite
            {
                UserId = userId,
                FilmId = request.MovieId
            };
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(IsFavorite), new { filmId = request.MovieId }, favorite);
        }
        [HttpDelete("Favorites/{filmId}")]
        public async Task<IActionResult> RemoveFavorite(int filmId)
        {
            var userId = GetUserId();
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.FilmId == filmId);

            if (favorite== null)
            {
                return NotFound(new { success = false, message = "Film favorilerinizde bulunamadı." });
            }
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("WatchLaters")]
        public async Task<IActionResult> GetWatchLaters()
        {
            var userId = GetUserId();
            var watchLaterFilmIds = await _context.WatchLaters
                .Where(w=> w.UserId == userId)
                .Select(w=> w.FilmId)
                .ToListAsync();
            return Ok(watchLaterFilmIds);
        }
        [HttpGet("WatchLaters/{filmId}")]
        public async Task<IActionResult> IsWatchLater(int filmId)
        {
            var userId = GetUserId();
            var isWatchLater = await _context.WatchLaters
                .AnyAsync(w => w.UserId == userId && w.FilmId == filmId);

            if (isWatchLater)
            {
                return Ok(new { isWatchLater = true });
            }
            return NotFound(new { isWatchLater = false });
        }
        [HttpPost("WatchLaters")]
        public async Task<IActionResult> AddWatchLater([FromBody] InteractionRequest request)
        {
            var userId = GetUserId();
            var exists = await _context.WatchLaters
                .AnyAsync(w => w.UserId == userId && w.FilmId == request.MovieId);

            if(exists)
            {
                return Conflict(new { success = false, message = "Film zaten izleme listenizde." });
            }
            var WatchLater = new WatchLater
            {
                UserId = userId,
                FilmId = request.MovieId
            };
            _context.WatchLaters.Add(WatchLater);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWatchLaters), new { filmId = request.MovieId }, WatchLater);
        }

        [HttpDelete("WatchLaters/{filmId}")]
        public async Task<IActionResult> RemoveWatchLater(int filmId)
        {
            var userId = GetUserId();
            var watchLater = await _context.WatchLaters
                .FirstOrDefaultAsync(w => w.UserId == userId && w.FilmId == filmId);

            if(watchLater == null)
            {
                return NotFound(new { success = false, message = "Film izleme listenizde bulunamadı." });
            }
            _context.WatchLaters.Remove(watchLater);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("UserList/AddItem")]
        public async Task<IActionResult> AddToList([FromBody] AddToListRequest request)
        {
            var userId = GetUserId();
            var listExists = await _context.UserLists
                .AnyAsync(l => l.Id == request.ListId && l.UserId == userId);

            if(!listExists)
            {
                return Forbid("Bu listeye erişiminiz yok.");
            }

            var movieExists = await _context.UserListItems
                .AnyAsync(i => i.UserListId == request.ListId && i.FilmId == request.MovieId);

            if (movieExists)
            {
                return Conflict("Film zaten listede.");
            }

            var listItem = new UserListItem
            {
                UserListId = request.ListId,
                FilmId = request.MovieId
            };

            _context.UserListItems.Add(listItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Film listeye eklendi." });
        }

        [HttpPost]
        [Route("Interactions/RemoveFromList/{listId}/{movieId}")]
        public async Task<IActionResult> RemoveFromList(int listId, int movieId)
        {
            var userId = GetUserId();
            var listItems = await _context.UserListItems
                .FirstOrDefaultAsync(li => li.UserListId == listId && li.FilmId == movieId && li.UserFilmList.UserId == userId);

            if(listItems == null)
            {
                return NotFound("Film listede bulunamadı");
            }

            _context.UserListItems.Remove(listItems);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "UserFilmList", new { id = listId });
        }

        [HttpGet("Ratings")]
        public async Task<IActionResult> GetRatings()
        {
            var userId = GetUserId();
            var ratingFilmIds = await _context.Ratings
                .Where(w => w.UserId == userId)
                .Select(w => w.FilmId)
                .ToListAsync();
            return Ok(ratingFilmIds);
        }
        [HttpGet("Ratings/{filmId}")]
        public async Task<IActionResult> inRating(int filmId)
        {
            var userId = GetUserId();
            var rating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.FilmId == filmId);

            if (rating != null)
            {
                return Ok(new { filmId = rating.FilmId, score = rating.Score });
            }
            return NotFound(new { message = "Bu film için bir puanlama bulunamadı." });
        }
        [HttpPost("Ratings")]
        public async Task<IActionResult> AddRating([FromBody] RatingRequest request)
        {
            var userId = GetUserId();
            var exists = await _context.Ratings
                .AnyAsync(r => r.UserId == userId && r.FilmId == request.MovieId);

            if (exists)
            {
                return Conflict(new { success = false, message = "Filmi zaten puanladın." });
            }
            var rating = new Rating
            {
                UserId = userId,
                FilmId = request.MovieId,
                Score = request.Rating
            };
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRatings), new { filmId = request.MovieId }, rating);
        }

        [HttpPut("Ratings")]
        public async Task<IActionResult> UpdateRating([FromBody] RatingRequest request)
        {
            if (request.Rating < 1 || request.Rating > 10)
            {
                return BadRequest("Puan 1 ile 10 arasında olmalıdır.");
            }

            var userId = GetUserId();
            var exists = await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.FilmId == request.MovieId);

            if (exists == null)
            {
                return NotFound("Güncellenecek puanlama bulunamadı.");
            }

            exists.Score = request.Rating;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Puan güncellendi.", score = exists.Score });
        }

        [HttpDelete("Ratings/{filmId}")]
        public async Task<IActionResult> RemoveRating(int filmId)
        {
            var userId = GetUserId();
            var rating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.FilmId == filmId);

            if (rating == null)
            {
                return NotFound(new { success = false, message = "Film izleme listenizde bulunamadı." });
            }
            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
public class InteractionRequest
{
    public int MovieId { get; set; }
}

public class AddToListRequest
{
    public int ListId { get; set; }
    public int MovieId { get; set; }
}
public class RatingRequest
{
    public int MovieId { get; set; }
    public int Rating { get; set; }
}
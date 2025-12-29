using MovieBuff.Services;
using Microsoft.AspNetCore.Mvc;
using MovieBuff.DTOs;
using MovieBuff.Data;
using System.Security.Claims;
using MovieBuff.ViewModels;
using Microsoft.EntityFrameworkCore;
namespace MovieBuff.Controllers
{
    public class FilmController: Controller
    {
        private readonly IMovieService _movieService;
        private readonly MovieBuffContext _context;
        public FilmController(IMovieService movieService,MovieBuffContext context)
        {
            _movieService = movieService;
            _context = context;
        }

        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieService.GetMovieDetailsAsync(id);
            
            if(movie == null)
            {
                return NotFound("Film bulunamadı.");
            }

            bool isFavorite = false;
            bool isInWatchLater = false;
            int? userRating = null;

            if(User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                isFavorite = await _context.Favorites
                    .AnyAsync(f => f.UserId == userId && f.FilmId == id);
                isInWatchLater = await _context.WatchLaters
                    .AnyAsync(w => w.UserId == userId && w.FilmId == id);
                var rating = await _context.Ratings
                    .FirstOrDefaultAsync(r => r.UserId == userId && r.FilmId == id);
                
                if(rating != null)
                {
                    userRating = rating.Score;
                }
            }

            var viewModel = new MovieDetailViewModel
            {
                Movie = movie,
                IsFavorite = isFavorite,
                IsInWatchLater = isInWatchLater,
                UserRating = userRating
            };
            return View(viewModel);
        }
    }
}

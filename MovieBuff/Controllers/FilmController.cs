using MovieBuff.Services;
using Microsoft.AspNetCore.Mvc;
using MovieBuff.DTOs;
namespace MovieBuff.Controllers
{
    public class FilmController: Controller
    {
        private readonly IMovieService _movieService;
        public FilmController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<IActionResult> Details(int Id)
        {
            if(Id == 0)
            {
                return BadRequest("Geçersiz bir id girdiniz.");
            }

            var movie = await _movieService.GetMovieDetailsAsync(Id);
            
            if(movie == null)
            {
                return NotFound("Film bulunamadı.");
            }
            return View(movie);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using MovieBuff.Services;
namespace MovieBuff.Controllers
{
    public class SearchController: Controller
    {
        private readonly IMovieService _movieService;

        public SearchController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<IActionResult> Results(string query, int page= 1)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index", "Home");
            }

            if (page < 1) { page = 1; }

            var searchResults =  await _movieService.SearchMoviesAsync(query, page);

            ViewBag.Query = query;

            return View(searchResults);
        }
    }
}

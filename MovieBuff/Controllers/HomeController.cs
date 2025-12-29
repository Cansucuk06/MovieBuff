using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MovieBuff.Models;
using MovieBuff.Services;
using MovieBuff.ViewModels;

namespace MovieBuff.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;

        public HomeController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        public async Task<IActionResult> Index(int page = 1, string genre = null, string? year = null, double? ImdbRating= null)
        {
            var popularMoviesTask =  _movieService.GetPopularMoviesAsync();
            var nowPlayingMoviesTask =  _movieService.GetNowPlayingMoviesAsync();
            var moviePage = await _movieService.DiscoverMoviesAsync(page, genre, year, ImdbRating);
            await Task.WhenAll(popularMoviesTask, nowPlayingMoviesTask);
            var viewModel = new HomeIndexViewModel
            {
                PagedMovieResults = moviePage,
                CurrentGenre = genre,
                CurrentYear = year,
                CurrentImdbRating = ImdbRating,
                NowPlayingMovies = await nowPlayingMoviesTask,
                PopularMovies = await popularMoviesTask
            };
            return View(viewModel);
        }
    }
}

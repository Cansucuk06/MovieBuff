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
        public async Task<IActionResult> Index()
        {
            var popularMoviesTask =  _movieService.GetPopularMoviesAsync();
            var nowPlayingMoviesTask =  _movieService.GetNowPlayingMoviesAsync();

            await Task.WhenAll(popularMoviesTask, nowPlayingMoviesTask);

            var viewModel = new HomeIndexViewModel
            {
                NowPlayingMovies = await nowPlayingMoviesTask,
                PopularMovies = await popularMoviesTask
            };
            return View(viewModel);
        }
    }
}

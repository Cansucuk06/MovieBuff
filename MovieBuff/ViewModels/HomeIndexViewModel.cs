using MovieBuff.DTOs;
namespace MovieBuff.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<MovieResultDto> PopularMovies { get; set; }
        public List<MovieResultDto> NowPlayingMovies { get; set; }
        public HomeIndexViewModel()
        {
            PopularMovies = new List<MovieResultDto>();
            NowPlayingMovies = new List<MovieResultDto>();
        }
    }
}

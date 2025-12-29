using MovieBuff.DTOs;
namespace MovieBuff.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<MovieResultDto> PopularMovies { get; set; }
        public List<MovieResultDto> NowPlayingMovies { get; set; }
        public PagedResult<MovieResultDto> PagedMovieResults{ get; set; }
        public string? CurrentGenre { get; set; }
        public string? CurrentYear { get; set; }
        public double? CurrentImdbRating { get; set; }
        public HomeIndexViewModel()
        {
            PopularMovies = new List<MovieResultDto>();
            NowPlayingMovies = new List<MovieResultDto>();
        }
    }
}

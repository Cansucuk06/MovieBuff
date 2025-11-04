using MovieBuff.DTOs;
namespace MovieBuff.Services
{
    public interface IMovieService
    {
        Task<List<MovieResultDto>> GetPopularMoviesAsync();
        Task<List<MovieResultDto>> GetNowPlayingMoviesAsync();
    }
}

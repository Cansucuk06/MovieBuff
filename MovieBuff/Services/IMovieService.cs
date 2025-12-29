using MovieBuff.DTOs;
namespace MovieBuff.Services
{
    
    public interface IMovieService
    {
        Task<List<MovieResultDto>> GetPopularMoviesAsync();
        Task<List<MovieResultDto>> GetNowPlayingMoviesAsync();
        Task<MovieDetailDto> GetMovieDetailsAsync(int movieId);
        Task<PagedResult<MovieResultDto>> DiscoverMoviesAsync(int page = 1, string? genre = null, string? year = null, double? ImdbRating = null);
        Task<PagedResult<MovieResultDto>> SearchMoviesAsync(string query, int page = 1);
    }
}

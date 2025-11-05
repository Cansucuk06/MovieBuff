using Microsoft.AspNetCore.Mvc;
using MovieBuff.DTOs;
using System.Text.Json;
namespace MovieBuff.Services
{
    public class MovieService: IMovieService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IConfiguration _configuration;

        public MovieService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiKey = _configuration["TMDB:ApiKey"];
        }

        public async Task<List<MovieResultDto>> GetPopularMoviesAsync()
        {
            var response = await _httpClient.GetAsync($"https://api.themoviedb.org/3/movie/popular?api_key={_apiKey}&language=en-US&page=1");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var movieResponse = JsonSerializer.Deserialize<MovieApiResponse>(content);
                return movieResponse?.Results ?? new List<MovieResultDto>();

            }
            return new List<MovieResultDto>();
        }

        public async Task<List<MovieResultDto>> GetNowPlayingMoviesAsync()
        {
            var response = await _httpClient.GetAsync($"https://api.themoviedb.org/3/movie/now_playing?api_key={_apiKey}&language=en-US&page=1");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var movieResponse = JsonSerializer.Deserialize<MovieApiResponse>(content);
                return movieResponse?.Results ?? new List<MovieResultDto>();
            }
            return new List<MovieResultDto>();
        }

        public async Task<MovieDetailDto> GetMovieDetailsAsync(int movieId)
        {
            var response = await _httpClient.GetAsync($"https://api.themoviedb.org/3/movie/{movieId}?api_key={_apiKey}&language=en-US&append_to_response=credits");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var movieDetails = JsonSerializer.Deserialize<MovieDetailDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return movieDetails;
            }
            return null;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using MovieBuff.DTOs;
using System.Text;
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

        public async Task<PagedResult<MovieResultDto>> DiscoverMoviesAsync(int page = 1, string? genre = null, string? year = null, double? ImdbRating= null)
        {
            var queryBuilder = new System.Text.StringBuilder();
            queryBuilder.Append($"https://api.themoviedb.org/3/discover/movie?api_key={_apiKey}&language=en-US&sort_by=popularity.desc");
            queryBuilder.Append($"&page={page}");
            if (!string.IsNullOrEmpty(genre))
            {
                queryBuilder.Append($"&with_genres={genre}");
            }

            if (!string.IsNullOrWhiteSpace(year) && int.TryParse(year, out int parsedYear))
            {
                if (parsedYear > 1874)
                {
                    queryBuilder.Append($"&primary_release_year={parsedYear}");
                }
            }

            if (ImdbRating.HasValue)
            {
                queryBuilder.Append($"&vote_average.gte={ImdbRating.Value}");
            }

            var response = await _httpClient.GetAsync(queryBuilder.ToString());
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<PagedResult<MovieResultDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return apiResponse;
            }
            return new PagedResult<MovieResultDto> { Results = new List<MovieResultDto>() };
        }

        public async Task<PagedResult<MovieResultDto>> SearchMoviesAsync(string query, int page= 1)
        {
            var trustedQuery = System.Net.WebUtility.UrlEncode(query);
            var url = $"https://api.themoviedb.org/3/search/movie?api_key={_apiKey}&language=en-US&query={trustedQuery}&page={page}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<PagedResult<MovieResultDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return apiResponse;
            }
            return new PagedResult<MovieResultDto> { Results = new List<MovieResultDto>(), Page= 1, TotalPages= 0};
        }
    }
}

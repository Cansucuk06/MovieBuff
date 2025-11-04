using System.Text.Json.Serialization;   
namespace MovieBuff.DTOs
{
    public class MovieApiResponse
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("results")]
        public List<MovieResultDto> Results { get; set; }
    }
}

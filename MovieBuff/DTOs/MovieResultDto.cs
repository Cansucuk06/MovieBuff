using System.Text.Json.Serialization;
namespace MovieBuff.DTOs
{
    public class MovieResultDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; }
        
        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; }

        [JsonPropertyName("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonPropertyName("vote_average")]
        public double VoteAvareage { get; set; }
    }
}

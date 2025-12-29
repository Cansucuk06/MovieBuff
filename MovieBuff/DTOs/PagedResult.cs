using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace MovieBuff.DTOs
{
    public class PagedResult<T>
    {
        [JsonPropertyName("results")]

        public List<T> Results { get; set; }
        [JsonPropertyName("page")]
        public int Page { get; set; }
        [JsonPropertyName("total_results")]

        public int TotalResults { get; set; }
        [JsonPropertyName("total_pages")]

        public int TotalPages { get; set; }
    }
}

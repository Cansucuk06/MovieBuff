using System.Text.Json.Serialization;
namespace MovieBuff.DTOs
{
    public class CrewMemberDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("job")]
        public string Job { get; set; }
    }
}

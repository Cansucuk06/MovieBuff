using System.Text.Json.Serialization;
namespace MovieBuff.DTOs
{
    public class CastMemberDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("character")]
        public string Character { get; set; }
        [JsonPropertyName("profile_path")]
        public string ProfilePath { get; set; }

    }
}

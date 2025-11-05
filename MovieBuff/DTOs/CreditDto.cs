using System.Text.Json.Serialization;

namespace MovieBuff.DTOs
{
    public class CreditDto
    {
        [JsonPropertyName("cast")]
        public List<CastMemberDto> Cast { get; set; }
        [JsonPropertyName("crew")]
        public List<CrewMemberDto> Crew { get; set; }
    }
}

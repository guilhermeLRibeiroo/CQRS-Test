using System.Text.Json.Serialization;

namespace Questao2.VOs
{
    public class MatchVO
    {
        [JsonPropertyName("team1")]
        public string Team1 { get; set; }

        [JsonPropertyName("team2")]
        public string Team2 { get; set; }

        [JsonPropertyName("team1goals")]
        public string Team1Goals { get; set; }

        [JsonPropertyName("team2goals")]
        public string Team2Goals { get; set; }
    }
}

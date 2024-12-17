using Questao2.VOs;
using System.Text.Json;

namespace Questao2.APIs
{
    public class FootballMatchesAPI
    {
        private static HttpClient httpClient = new()
        {
            BaseAddress = new Uri("https://jsonmock.hackerrank.com/")
        };

        public static async Task<int> GetTeamGoalsByYear(string team, int year)
        {
            var matches = await GetMatches(team, year, page: 1);
            var total_goals = matches.Data.Select(d => int.Parse(d.Team1Goals)).Sum();

            for (int i = 2; i < matches.TotalPages+1; i++)
            {
                var matches1 = await GetMatches(team, year, page: i);
                total_goals += matches1.Data.Select(d => int.Parse(d.Team1Goals)).Sum();
            }

            return total_goals;
        }

        private static async Task<FootballMatchesResponse> GetMatches(string team, int year, int page)
        {
            var response = await httpClient.GetAsync($"api/football_matches?year={year}&team1={team}&page={page}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Get 'api/football_matches' resulted in {response.StatusCode} status code.");

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<FootballMatchesResponse>(content);
        }
    }
}

using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InfoTrackScraper.Services
{
    public class GoogleSearchService
    {
        public async Task<int> PerformSearchAsync(string keyword, string targetUrl)
        {
            var formattedKeyword = keyword.Replace(" ", "+");
            var searchUrl = $"https://www.google.co.uk/search?num=100&q={formattedKeyword}";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(searchUrl);

            var pattern = $"href=\\\"{Regex.Escape(targetUrl)}";
            var regex = new Regex(pattern);
            var matches = regex.Matches(response);
            return matches.Count;

        }
    }
}
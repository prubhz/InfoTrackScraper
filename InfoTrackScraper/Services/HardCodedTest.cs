using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace InfoTrackScraper.Services
{
    public class HardCodedTest
    {
        public async Task<int> PerformSearchAsync(string keyword, string targetUrl)
        {
            // This test searches a saved copy of a previous search via a web browser.
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Services", "assets", "land registry search - Google Search.html");
            var response = await File.ReadAllTextAsync(filePath);
            var pattern = $"href=\\\"{Regex.Escape(targetUrl)}"; 
            var regex = new Regex(pattern);
            var matches = regex.Matches(response);
            return matches.Count;
        }
    }
}
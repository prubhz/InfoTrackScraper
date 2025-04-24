using System.Diagnostics;

namespace InfoTrackScraper.Services
{
    public class NativeBrowserSearchService
    {
        public void PerformSearch(string keyword)
        {
            // Attempt to open the search URL in the default web browser
            // Fails as we cannot extract information from this process
            var searchUrl = $"https://www.google.com/search?q={Uri.EscapeDataString(keyword)}";
            Process.Start(new ProcessStartInfo
            {
                FileName = "msedge",
                Arguments = searchUrl,
                UseShellExecute = true
            });
        }
    }
}
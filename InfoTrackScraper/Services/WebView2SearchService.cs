using Microsoft.Web.WebView2.WinForms;

namespace InfoTrackScraper.Services
{
    public class WebView2SearchService
    {
        public async Task<string> PerformSearchAsync(string keyword)
        {
            var webView = new WebView2
            {
                Source = new Uri($"https://www.google.com/search?q={Uri.EscapeDataString(keyword)}")
            };

            // Wait for CoreWebView2 to initialize
            var tcs = new TaskCompletionSource();
            webView.CoreWebView2InitializationCompleted += (sender, args) =>
            {
                if (args.IsSuccess)
                {
                    tcs.SetResult();
                }
                else
                {
                    tcs.SetException(new InvalidOperationException("WebView2 initialization failed."));
                }
            };

            await tcs.Task; // Wait for initialization to complete

            // Wait for the page to load
            await Task.Delay(5000);

            // Execute JavaScript to extract search results
            string script = @"
                Array.from(document.querySelectorAll('a'))
                     .map(a => a.href)
                     .filter(href => href.startsWith('http'))
                     .join('\n');
            ";
            string result = await webView.CoreWebView2.ExecuteScriptAsync(script);

            return result;
        }
    }
}
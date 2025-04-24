using System.Net.Http;
using System.Net.Http.Json; // Required for ReadFromJsonAsync
using System.Text.Json.Serialization; // Required for JsonPropertyName
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration; // Required for IConfiguration

namespace InfoTrackScraper.Services
{
    // Models to represent the Google Custom Search API response
    public class GoogleSearchResponse
    {
        [JsonPropertyName("items")]
        public List<SearchResultItem>? Items { get; set; }
    }

    public class SearchResultItem
    {
        [JsonPropertyName("link")]
        public string? Link { get; set; }
    }

    public class CustomSearchApiService // Renamed class for clarity
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private const string BaseUrl = "https://www.googleapis.com/customsearch/v1";

        // Inject HttpClientFactory and IConfiguration
        public CustomSearchApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        public async Task<int> SearchAsync(string keyword, string urlToMatch)
        {
            var apiKey = _configuration["ApiKeys:CustomSearchApi"];
            // Construct the request URL
            var requestUrl = $"{BaseUrl}?key={apiKey}&q={Uri.EscapeDataString(keyword)}&num=100"; 

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode(); // Throw if HTTP request failed

                var searchResponse = await response.Content.ReadFromJsonAsync<GoogleSearchResponse>();

                if (searchResponse?.Items == null || !searchResponse.Items.Any())
                {
                    return 0; // No results found
                }

                int count = 0;
                foreach (var item in searchResponse.Items)
                {
                    // Check if the result link contains the URL to match (case-insensitive)
                    if (item.Link != null && item.Link.Contains(urlToMatch, StringComparison.OrdinalIgnoreCase))
                    {
                        count++;
                    }
                }
                return count;
            }
            catch (HttpRequestException e)
            {
                Console.Error.WriteLine($"HTTP Request Error: {e.Message}");
                return -1;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"An error occurred: {e.Message}");
                return -1;
            }
        }
    }
}

using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using InfoTrackScraper.Services;

namespace InfoTrackScraper.Application.Scrape.Queries
{
    public class ScrapeQueryHandler : IRequestHandler<ScrapeQuery, int>
    {
        private readonly HardCodedTest _hardCodeTest;
        private readonly GoogleSearchService _googleSearchService;
        private readonly CustomSearchApiService _customSearchApiService;
        private readonly NativeBrowserSearchService _nativeBrowserSearch;
        private readonly WebView2SearchService _searchService;

        public ScrapeQueryHandler(HardCodedTest hardCodedTest,
                                  GoogleSearchService googleSearchService,
                                  CustomSearchApiService customSearchApiService,
                                  WebView2SearchService searchService,
                                  NativeBrowserSearchService nativeBrowserSearch)
        {
            _hardCodeTest = hardCodedTest;
            _googleSearchService = googleSearchService;
            _customSearchApiService = customSearchApiService;
            _nativeBrowserSearch = nativeBrowserSearch;
            _searchService = searchService;
        }

        public async Task<int> Handle(ScrapeQuery request, CancellationToken cancellationToken)
        {
            var response = await _hardCodeTest.PerformSearchAsync(request.Keyword, request.Url);

            return response;
        }
    }
}

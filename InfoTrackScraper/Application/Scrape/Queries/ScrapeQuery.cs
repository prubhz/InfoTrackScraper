using System;
using MediatR;

namespace InfoTrackScraper.Application.Scrape.Queries
{
    public class ScrapeQuery : IRequest<int>
    {
        public int Id { get; set; }
        public required string Keyword { get; set; }
        public required string Url { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

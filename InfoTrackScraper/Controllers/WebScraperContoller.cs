using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.RegularExpressions;
using FluentValidation;
using InfoTrackScraper.Validators;
using InfoTrackScraper.Models;
using InfoTrackScraper.Services;
using MediatR;
using InfoTrackScraper.Application.Scrape.Queries;

namespace InfoTrackScraper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebScraperContoller : ControllerBase
    {
        private readonly ScrapeRequestValidator _validator;
        private readonly IMediator _mediator;

        public WebScraperContoller(ScrapeRequestValidator validator, IMediator mediator)
        {
            _validator = validator;
            _mediator = mediator;
        }

        [HttpPost("Scrape")]
        public async Task<IActionResult> Scrape([FromBody] ScrapeRequest request)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var query = new ScrapeQuery
            {
                Keyword = request.Keyword,
                Url = request.Url
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}

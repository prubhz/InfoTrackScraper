using FluentValidation;
using InfoTrackScraper.Models;

namespace InfoTrackScraper.Validators
{
    public class ScrapeRequestValidator : AbstractValidator<ScrapeRequest>
    {
        public ScrapeRequestValidator()
        {
            RuleFor(request => request.Keyword)
                .NotEmpty().WithMessage("Keyword is required.");

            RuleFor(request => request.Url)
                .NotEmpty().WithMessage("Url is required.");
        }
    }
}

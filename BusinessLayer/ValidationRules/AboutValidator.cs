using EntityLayer.Concrate;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class AboutValidator : AbstractValidator<About>
    {
        public AboutValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Bitte geben Sie einen Titel ein.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Bitte geben Sie eine Beschreibung ein.");
            RuleFor(x => x.Title2).NotEmpty().WithMessage("Bitte geben Sie den zweiten Titel ein.");
            RuleFor(x => x.Description2).NotEmpty().WithMessage("Bitte geben Sie die zweite Beschreibung ein.");
        }
    }
}

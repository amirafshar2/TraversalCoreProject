using EntityLayer.Concrate;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class GuideValidator : AbstractValidator<Guide>
    {
        public GuideValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Bitte geben Sie den Namen des Reiseführers ein.")
                .MaximumLength(60).WithMessage("Der Name darf höchstens 60 Zeichen lang sein.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Bitte geben Sie eine Beschreibung ein.")
                .MaximumLength(600).WithMessage("Die Beschreibung darf höchstens 600 Zeichen lang sein.");
        }
    }
}

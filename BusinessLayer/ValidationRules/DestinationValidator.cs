using EntityLayer.Concrate;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class DestinationValidator : AbstractValidator<Destiniton>
    {
        public DestinationValidator()
        {
            RuleFor(x => x.City).NotEmpty().WithMessage("Bitte geben Sie eine Stadt ein.");
            RuleFor(x => x.DayNight).NotEmpty().WithMessage("Bitte geben Sie die Dauer (Tage/Nächte) ein.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Der Preis muss größer als 0 sein.");
            RuleFor(x => x.Capacity).GreaterThan(0).WithMessage("Die Kapazität muss größer als 0 sein.");
        }
    }
}

using EntityLayer.Concrate;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class ContactMessageValidator : AbstractValidator<ContactMessage>
    {
        public ContactMessageValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Bitte geben Sie Ihren Namen ein.").MaximumLength(80);
            RuleFor(x => x.Mail).NotEmpty().WithMessage("Bitte geben Sie Ihre E-Mail-Adresse ein.")
                .EmailAddress().WithMessage("Bitte geben Sie eine gültige E-Mail-Adresse ein.");
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Bitte geben Sie einen Betreff ein.").MaximumLength(120);
            RuleFor(x => x.Message).NotEmpty().WithMessage("Bitte schreiben Sie eine Nachricht.")
                .MinimumLength(10).WithMessage("Die Nachricht muss mindestens 10 Zeichen lang sein.")
                .MaximumLength(2000);
        }
    }
}

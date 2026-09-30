using System.ComponentModel.DataAnnotations;

namespace TraversalCoreProje.Models
{
    public class Usermodel
    {
        [Required(ErrorMessage = "Bitte geben Sie Ihren Vornamen ein.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie Ihren Nachnamen ein.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie Ihre E-Mail-Adresse ein.")]
        [EmailAddress(ErrorMessage = "Bitte geben Sie eine gültige E-Mail-Adresse ein.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie Ihre Telefonnummer ein.")]
        public string Telefonno { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie ein Passwort ein.")]
        [MinLength(6, ErrorMessage = "Das Passwort muss mindestens 6 Zeichen lang sein.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bitte bestätigen Sie Ihr Passwort.")]
        [Compare("Password", ErrorMessage = "Die Passwörter stimmen nicht überein.")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "Bitte wählen Sie Ihr Geschlecht.")]
        public string Gender { get; set; }

        /// <summary>Profilbild (optional).</summary>
        public IFormFile imagefile { get; set; }
    }
}

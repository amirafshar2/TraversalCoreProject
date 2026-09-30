using System.ComponentModel.DataAnnotations;

namespace TraversalCoreProje.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Bitte geben Sie Ihre E-Mail-Adresse ein.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie Ihr Passwort ein.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.Controllers
{
    [AllowAnonymous]
    public class NewsletterController : Controller
    {
        private readonly INewsletterService _newsletter;

        public NewsletterController(INewsletterService newsletter)
        {
            _newsletter = newsletter;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Subscribe(string email, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
                TempData["newsletter"] = "Bitte geben Sie eine gültige E-Mail-Adresse ein.";
            else if (_newsletter.Subscribe(email))
                TempData["newsletter"] = "Danke! Sie haben den Newsletter erfolgreich abonniert.";
            else
                TempData["newsletter"] = "Diese E-Mail-Adresse ist bereits eingetragen.";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl + "#newsletter");
            return RedirectToAction("Index", "Default");
        }
    }
}

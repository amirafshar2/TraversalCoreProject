using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TraversalCoreProje.Infrastructure;

namespace TraversalCoreProje.Controllers
{
    /// <summary>
    /// Öffentlicher Demo-Zugang: Besucher (z. B. vom Portfolio) können sich mit einem Klick
    /// ohne Registrierung als Admin oder als Kunde anmelden und die Anwendung ausprobieren.
    /// </summary>
    [AllowAnonymous]
    public class DemoController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly DemoOptions _demo;

        public DemoController(SignInManager<User> signInManager, UserManager<User> userManager, IOptions<DemoOptions> demo)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _demo = demo.Value;
        }

        /// <summary>Startseite der Demo mit Auswahl Admin / Kunde.</summary>
        public IActionResult Index()
        {
            if (!_demo.Enabled) return NotFound();
            return View(_demo);
        }

        /// <summary>/Demo/Admin → direkt in das Admin-Panel.</summary>
        public Task<IActionResult> Admin() =>
            SignInAs(_demo.AdminEmail, Url.Action("Index", "Dashbord", new { area = "Admin" }));

        /// <summary>/Demo/Member → direkt in den Kundenbereich.</summary>
        public Task<IActionResult> Member() =>
            SignInAs(_demo.MemberEmail, Url.Action("Index", "Dashboard", new { area = "Member" }));

        private async Task<IActionResult> SignInAs(string email, string target)
        {
            if (!_demo.Enabled) return NotFound();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return RedirectToAction("Login", "User");

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, isPersistent: false);
            TempData["success"] = $"Willkommen in der Demo! Sie sind als {user.Name} {user.Surname} angemeldet.";
            return LocalRedirect(target);
        }
    }
}

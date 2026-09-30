using EntityLayer.Concrate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TraversalCoreProje.Areas.Member.Models;
using TraversalCoreProje.Infrastructure;
using TraversalCoreProje.Models.PicMethods;

namespace TraversalCoreProje.Areas.Member.Controllers
{
    [Area("Member")]
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _usermanager;
        private readonly SignInManager<User> _signInManager;
        private readonly DemoOptions _demo;
        private readonly PicSave _pic = new();

        public ProfileController(UserManager<User> usermanager, SignInManager<User> signInManager, IOptions<DemoOptions> demo)
        {
            _usermanager = usermanager;
            _signInManager = signInManager;
            _demo = demo.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _usermanager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "User", new { area = "" });
            ViewBag.IsProtected = _demo.IsProtectedAccount(user.Email);
            ViewBag.Roles = await _usermanager.GetRolesAsync(user);
            ViewBag.CreatedAt = user.CreatedAt;
            return View(ToModel(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserModel model)
        {
            var u = await _usermanager.GetUserAsync(User);
            if (u == null) return RedirectToAction("Login", "User", new { area = "" });

            if (!PicSave.IsValidImage(model.ImageFile, out var err))
            {
                TempData["error"] = err;
                return RedirectToAction(nameof(Index));
            }
            if (string.IsNullOrWhiteSpace(model.name) || string.IsNullOrWhiteSpace(model.surename))
            {
                TempData["error"] = "Vor- und Nachname dürfen nicht leer sein.";
                return RedirectToAction(nameof(Index));
            }

            u.Name = model.name.Trim();
            u.Surname = model.surename.Trim();
            u.PhoneNumber = model.phone;
            u.gender = model.gender;
            u.Image = await _pic.SaveFileAsync(model.ImageFile) ?? u.Image;

            // Die E-Mail der Demo-Konten bleibt fest, damit der Demo-Login immer funktioniert
            if (!_demo.IsProtectedAccount(u.Email) && !string.IsNullOrWhiteSpace(model.email) &&
                !string.Equals(u.Email, model.email, StringComparison.OrdinalIgnoreCase))
            {
                u.Email = model.email.Trim();
                u.UserName = model.email.Trim();
            }

            var result = await _usermanager.UpdateAsync(u);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(u);
                TempData["success"] = "Ihr Profil wurde gespeichert.";
            }
            else
            {
                TempData["error"] = string.Join(" ", result.Errors.Select(e => e.Description));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(UserModel p)
        {
            var u = await _usermanager.GetUserAsync(User);
            if (u == null) return RedirectToAction("Login", "User", new { area = "" });

            if (_demo.IsProtectedAccount(u.Email))
                TempData["error"] = "Im Demo-Modus kann das Passwort dieses Kontos nicht geändert werden.";
            else if (string.IsNullOrEmpty(p.password) || p.password != p.passwordconfirm)
                TempData["error"] = "Die neuen Passwörter stimmen nicht überein.";
            else
            {
                var result = await _usermanager.ChangePasswordAsync(u, p.passwordcurrent ?? "", p.password);
                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(u);
                    TempData["success"] = "Ihr Passwort wurde geändert.";
                }
                else TempData["error"] = string.Join(" ", result.Errors.Select(e => e.Description));
            }
            return RedirectToAction(nameof(Index));
        }

        private static UserModel ToModel(User user) => new()
        {
            id = user.Id,
            email = user.Email,
            name = user.Name,
            surename = user.Surname,
            phone = user.PhoneNumber,
            gender = user.gender,
            image = user.Image
        };
    }
}

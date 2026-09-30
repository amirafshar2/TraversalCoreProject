using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TraversalCoreProje.Infrastructure;
using TraversalCoreProje.Models;
using TraversalCoreProje.Models.PicMethods;

namespace TraversalCoreProje.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly DemoOptions _demo;
        private readonly PicSave _pic = new();

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IOptions<DemoOptions> demo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _demo = demo.Value;
        }

        #region Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.Demo = _demo;
            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel p)
        {
            ViewBag.Demo = _demo;
            if (!ModelState.IsValid) return View(p);

            var result = await _signInManager.PasswordSignInAsync(p.Username, p.Password, p.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(p.ReturnUrl) && Url.IsLocalUrl(p.ReturnUrl))
                    return LocalRedirect(p.ReturnUrl);

                var user = await _userManager.FindByNameAsync(p.Username);
                if (await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Moderator"))
                    return RedirectToAction("Index", "Dashbord", new { area = "Admin" });
                return RedirectToAction("Index", "Dashboard", new { area = "Member" });
            }

            ModelState.AddModelError("", result.IsLockedOut
                ? "Das Konto ist wegen zu vieler Fehlversuche vorübergehend gesperrt."
                : "E-Mail-Adresse oder Passwort ist falsch.");
            return View(p);
        }
        #endregion

        #region Registrierung
        [HttpGet]
        public IActionResult registor()
        {
            return View(new Usermodel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> registor(Usermodel p)
        {
            if (!PicSave.IsValidImage(p.imagefile, out var imageError))
                ModelState.AddModelError(nameof(p.imagefile), imageError);
            if (!ModelState.IsValid) return View(p);

            var user = new User
            {
                Name = p.Name,
                Surname = p.Surname,
                Email = p.Email,
                PhoneNumber = p.Telefonno,
                UserName = p.Email,
                gender = p.Gender,
                CreatedAt = DateTime.Now,
                Image = await _pic.SaveFileAsync(p.imagefile) ?? "/otika-bootstrap-admin-template/assets/img/users/user-2.png"
            };

            var result = await _userManager.CreateAsync(user, p.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Member");
                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["success"] = "Ihr Konto wurde erstellt. Willkommen bei Traversal!";
                return RedirectToAction("Index", "Dashboard", new { area = "Member" });
            }

            foreach (var item in result.Errors)
                ModelState.AddModelError("", item.Description);
            return View(p);
        }
        #endregion

        #region Logout / Zugriff verweigert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Default");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion
    }
}

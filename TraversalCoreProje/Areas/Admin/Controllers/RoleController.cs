using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraversalCoreProje.Areas.Admin.Models;

namespace TraversalCoreProje.Areas.Admin.Controllers
{
    /// <summary>Rollenverwaltung (ASP.NET Core Identity).</summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private static readonly string[] SystemRoles = { "Admin", "Moderator", "Member" };

        private readonly RoleManager<Roll> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleController(RoleManager<Roll> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.AsNoTracking().OrderBy(r => r.Name).ToListAsync();
            var list = new List<RoleViewModel>();
            foreach (var r in roles)
            {
                list.Add(new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    UserCount = (await _userManager.GetUsersInRoleAsync(r.Name)).Count,
                    IsSystemRole = SystemRoles.Contains(r.Name)
                });
            }
            return View(list);
        }

        [HttpGet]
        public IActionResult Create() => View("Form", new RoleViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (!ModelState.IsValid) return View("Form", model);
            var result = await _roleManager.CreateAsync(new Roll(model.Name.Trim()) { Description = model.Description });
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
                return View("Form", model);
            }
            TempData["success"] = $"Die Rolle „{model.Name}“ wurde angelegt.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var r = await _roleManager.FindByIdAsync(id.ToString());
            if (r == null) return NotFound();
            return View("Form", new RoleViewModel { Id = r.Id, Name = r.Name, Description = r.Description, IsSystemRole = SystemRoles.Contains(r.Name) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleViewModel model)
        {
            var r = await _roleManager.FindByIdAsync(model.Id.ToString());
            if (r == null) return NotFound();
            model.IsSystemRole = SystemRoles.Contains(r.Name);
            if (model.IsSystemRole && r.Name != model.Name)
                ModelState.AddModelError(nameof(model.Name), "Systemrollen können nicht umbenannt werden.");
            if (!ModelState.IsValid) return View("Form", model);

            r.Name = model.Name.Trim();
            r.Description = model.Description;
            var result = await _roleManager.UpdateAsync(r);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
                return View("Form", model);
            }
            TempData["success"] = $"Die Rolle „{r.Name}“ wurde gespeichert.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var r = await _roleManager.FindByIdAsync(id.ToString());
            if (r == null) return RedirectToAction(nameof(Index));
            if (SystemRoles.Contains(r.Name))
            {
                TempData["error"] = "Systemrollen (Admin, Moderator, Member) können nicht gelöscht werden.";
                return RedirectToAction(nameof(Index));
            }
            await _roleManager.DeleteAsync(r);
            TempData["success"] = $"Die Rolle „{r.Name}“ wurde gelöscht.";
            return RedirectToAction(nameof(Index));
        }
    }
}

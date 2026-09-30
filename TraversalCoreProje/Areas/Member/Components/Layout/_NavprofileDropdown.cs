using EntityLayer.Concrate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.Areas.Member.Components.Layout
{
    /// <summary>Profil-Dropdown oben rechts im Panel.</summary>
    public class _NavprofileDropdown : ViewComponent
    {
        private readonly UserManager<User> _userManager;

        public _NavprofileDropdown(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var q = await _userManager.GetUserAsync(UserClaimsPrincipal);
            if (q != null)
            {
                ViewBag.id = q.Id;
                ViewBag.name = q.Name;
                ViewBag.surename = q.Surname;
                ViewBag.image = q.Image;
                ViewBag.email = q.Email;
            }
            return View();
        }
    }
}

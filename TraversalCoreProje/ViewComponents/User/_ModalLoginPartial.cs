using EntityLayer.Concrate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.ViewComponents.User
{
    /// <summary>Anmelde-Button bzw. Benutzermenü im Header der Website.</summary>
    public class _ModalLoginPartial : ViewComponent
    {
        private readonly UserManager<EntityLayer.Concrate.User> _usermanager;

        public _ModalLoginPartial(UserManager<EntityLayer.Concrate.User> usermanager)
        {
            _usermanager = usermanager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _usermanager.GetUserAsync(UserClaimsPrincipal);
                if (user != null)
                {
                    ViewBag.name = user.Name;
                    ViewBag.Surname = user.Surname;
                    ViewBag.Image = user.Image;
                    ViewBag.IsAdmin = await _usermanager.IsInRoleAsync(user, "Admin") || await _usermanager.IsInRoleAsync(user, "Moderator");
                }
            }
            return View();
        }
    }
}

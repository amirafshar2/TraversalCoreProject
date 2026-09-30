using BusinessLayer.Abstract;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TraversalCoreProje.Areas.Admin.Models;
using TraversalCoreProje.Infrastructure;
using TraversalCoreProje.Models.PicMethods;
using X.PagedList;
using X.PagedList.Extensions;

namespace TraversalCoreProje.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Roll> _roleManager;
        private readonly ICommentService _comment;
        private readonly IReservationService _reservation;
        private readonly DemoOptions _demo;
        private readonly PicSave _pic = new();

        public UserController(UserManager<User> userManager, RoleManager<Roll> roleManager, ICommentService comment,
            IReservationService reservation, IOptions<DemoOptions> demo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _comment = comment;
            _reservation = reservation;
            _demo = demo.Value;
        }

        #region Liste
        public async Task<IActionResult> Index(string q, int page = 1)
        {
            var query = _userManager.Users.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var t = q.Trim().ToLower();
                query = query.Where(u => u.Name.ToLower().Contains(t) || u.Surname.ToLower().Contains(t) || u.Email.ToLower().Contains(t));
            }
            var users = await query.OrderByDescending(u => u.CreatedAt).ToListAsync();
            var reservations = _reservation.GetAll();
            var comments = _comment.GetAllForAdmin();

            var list = new List<UserListItem>();
            foreach (var u in users)
            {
                list.Add(new UserListItem
                {
                    Id = u.Id,
                    FullName = $"{u.Name} {u.Surname}",
                    Email = u.Email,
                    Phone = u.PhoneNumber,
                    Image = u.Image,
                    Gender = u.gender,
                    CreatedAt = u.CreatedAt,
                    Roles = (await _userManager.GetRolesAsync(u)).ToList(),
                    ReservationCount = reservations.Count(r => r.Userid == u.Id),
                    CommentCount = comments.Count(c => c.Userid == u.Id),
                    IsProtected = _demo.IsProtectedAccount(u.Email)
                });
            }
            ViewBag.Search = q;
            return View(list.ToPagedList(page, 10));
        }
        #endregion

        #region Bearbeiten
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();
            return View(new UserModel
            {
                id = user.Id,
                name = user.Name,
                surename = user.Surname,
                email = user.Email,
                phone = user.PhoneNumber,
                image = user.Image,
                gender = user.gender,
                Roles = (await _userManager.GetRolesAsync(user)).ToList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserModel model)
        {
            var u = await _userManager.FindByIdAsync(model.id.ToString());
            if (u == null) return NotFound();
            if (!PicSave.IsValidImage(model.ImageFile, out var err)) ModelState.AddModelError(nameof(model.ImageFile), err);
            if (_demo.IsProtectedAccount(u.Email) && !string.Equals(u.Email, model.email, StringComparison.OrdinalIgnoreCase))
                ModelState.AddModelError(nameof(model.email), "Die E-Mail-Adresse der Demo-Konten kann nicht geändert werden.");
            if (!ModelState.IsValid)
            {
                model.image = u.Image;
                model.Roles = (await _userManager.GetRolesAsync(u)).ToList();
                return View(model);
            }

            u.Name = model.name;
            u.Surname = model.surename;
            u.PhoneNumber = model.phone;
            u.gender = model.gender;
            if (!string.Equals(u.Email, model.email, StringComparison.OrdinalIgnoreCase))
            {
                u.Email = model.email;
                u.UserName = model.email;
            }
            u.Image = await _pic.SaveFileAsync(model.ImageFile) ?? u.Image;

            var result = await _userManager.UpdateAsync(u);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
                model.image = u.Image;
                return View(model);
            }
            TempData["success"] = $"Der Benutzer {u.Name} {u.Surname} wurde gespeichert.";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Löschen
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return RedirectToAction(nameof(Index));

            if (_demo.IsProtectedAccount(user.Email) || user.Id == int.Parse(_userManager.GetUserId(User)))
            {
                TempData["error"] = "Dieses Konto kann nicht gelöscht werden (Demo-Konto bzw. eigenes Konto).";
                return RedirectToAction(nameof(Index));
            }
            await _userManager.DeleteAsync(user);
            TempData["success"] = $"Der Benutzer {user.Name} {user.Surname} wurde gelöscht.";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Rollen zuweisen
        [HttpGet]
        public async Task<IActionResult> AssignRole(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();
            var userRoles = await _userManager.GetRolesAsync(user);
            var model = new RoleAssignViewModel
            {
                UserId = user.Id,
                FullName = $"{user.Name} {user.Surname}",
                Image = user.Image,
                Roles = _roleManager.Roles.AsNoTracking().OrderBy(r => r.Name).ToList()
                    .Select(r => new RoleCheck { RoleId = r.Id, Name = r.Name, Description = r.Description, Exists = userRoles.Contains(r.Name) })
                    .ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(RoleAssignViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null) return NotFound();

            if (_demo.IsProtectedAccount(user.Email))
            {
                TempData["error"] = "Die Rollen der Demo-Hauptkonten können nicht geändert werden.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var r in model.Roles)
            {
                if (r.Exists) await _userManager.AddToRoleAsync(user, r.Name);
                else await _userManager.RemoveFromRoleAsync(user, r.Name);
            }
            TempData["success"] = $"Die Rollen von {user.Name} {user.Surname} wurden aktualisiert.";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Kommentare eines Benutzers
        public async Task<IActionResult> Comments(int id, int page = 1)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();

            ViewBag.UserName = $"{user.Name} {user.Surname}";
            ViewBag.UserImage = user.Image;
            ViewBag.UserMail = user.Email;
            ViewBag.Userid = user.Id;
            ViewBag.ReservationCount = _reservation.GetlistByuseridaccept(id).Count;

            var models = _comment.GetCommentsByUserID(id).Select(item => new CommentWithDestinationundUserModel
            {
                id = item.id,
                CommentContent = item.CommentContent,
                CommentData = item.CommentData,
                CommentUser = item.CommentUser,
                DestinationID = item.Destinitonid,
                UserID = item.Userid,
                UserImage = user.Image,
                UserName = user.Name,
                UserSurname = user.Surname,
                status = item.status,
                DesCity = item.Destiniton?.City,
                DesImage = item.Destiniton?.Image
            }).ToList();
            ViewBag.CountComment = models.Count;
            return View(models.ToPagedList(page, 6));
        }
        #endregion
    }
}

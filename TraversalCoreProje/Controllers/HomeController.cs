using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraversalCoreProje.Models;

namespace TraversalCoreProje.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public IActionResult Index() => RedirectToAction("Index", "Default");

        public IActionResult Privacy() => RedirectToAction("Index", "Legal");

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>Freundliche Seite für 404/403 usw.</summary>
        public IActionResult StatusCode(int code)
        {
            ViewBag.Code = code;
            return View();
        }
    }
}

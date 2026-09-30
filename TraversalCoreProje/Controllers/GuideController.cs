using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.Controllers
{
    [AllowAnonymous]
    public class GuideController : Controller
    {
        private readonly IGuideService _guide;

        public GuideController(IGuideService guide)
        {
            _guide = guide;
        }

        public IActionResult Index()
        {
            return View(_guide.GetActive());
        }
    }
}

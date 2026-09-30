using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TraversalCoreProje.Infrastructure;

namespace TraversalCoreProje.Controllers
{
    /// <summary>Hinweise zum Demo-Projekt, Impressum und Datenschutz.</summary>
    [AllowAnonymous]
    public class LegalController : Controller
    {
        private readonly DemoOptions _demo;

        public LegalController(IOptions<DemoOptions> demo)
        {
            _demo = demo.Value;
        }

        public IActionResult Index() => View(_demo);
    }
}

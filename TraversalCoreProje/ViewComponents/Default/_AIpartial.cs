using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.ViewComponents.Default
{
    /// <summary>KI-Stadtinfo-Assistent in der Navigationsleiste des Panels.</summary>
    public class _AIpartial : ViewComponent
    {
        private readonly IConfiguration _config;

        public _AIpartial(IConfiguration config)
        {
            _config = config;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.Enabled = !string.IsNullOrWhiteSpace(_config["Gemini:ApiKey"]);
            return View();
        }
    }
}

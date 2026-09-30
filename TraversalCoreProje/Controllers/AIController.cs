using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.Controllers
{
    /// <summary>
    /// KI-Reiseinfos über die Google-Gemini-API.
    /// Der API-Schlüssel wird NICHT im Code gespeichert, sondern über die Konfiguration
    /// (User-Secrets oder Umgebungsvariable "Gemini__ApiKey") gesetzt. Ohne Schlüssel ist die Funktion deaktiviert.
    /// </summary>
    public class AIController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public AIController(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetCityInfo([FromBody] CityRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.CityName) || request.CityName.Length > 60)
                return BadRequest("Bitte geben Sie einen gültigen Städtenamen ein.");

            var apiKey = _config["Gemini:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                return StatusCode(503, "Der KI-Assistent ist in dieser Demo deaktiviert (kein API-Schlüssel konfiguriert).");

            var model = _config["Gemini:Model"] ?? "gemini-2.0-flash";
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent";
            var prompt = $"Gib auf Deutsch 7 kurze Reisetipps für die Stadt {request.CityName}. " +
                         "Jeder Tipp hat einen Titel und eine kurze Beschreibung (max. 2 Sätze). " +
                         "Antworte ausschließlich als JSON-Array im Format: " +
                         "[{\"title\": \"...\", \"description\": \"...\"}]";

            var body = new { contents = new[] { new { parts = new[] { new { text = prompt } } } } };
            var client = _httpClientFactory.CreateClient();
            using var msg = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };
            msg.Headers.Add("x-goog-api-key", apiKey);

            var response = await client.SendAsync(msg);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Die KI-Informationen konnten nicht geladen werden.");

            try
            {
                var gemini = JsonSerializer.Deserialize<GeminiResponse>(await response.Content.ReadAsStringAsync());
                var text = gemini?.candidates?[0].content?.parts?[0].text ?? "[]";
                var clean = text.Replace("```json", "").Replace("```", "").Trim();
                return Ok(JsonSerializer.Deserialize<List<CityInfo>>(clean));
            }
            catch (JsonException)
            {
                return StatusCode(502, "Die Antwort der KI konnte nicht gelesen werden.");
            }
        }
    }

    #region Request- und Response-Modelle
    public class CityRequest { public string CityName { get; set; } }
    public class GeminiResponse { public List<Candidate> candidates { get; set; } }
    public class Candidate { public Content content { get; set; } }
    public class Content { public List<Part> parts { get; set; } }
    public class Part { public string text { get; set; } }
    public class CityInfo { public string title { get; set; } public string description { get; set; } }
    #endregion
}

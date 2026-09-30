namespace TraversalCoreProje.Models.PicMethods
{
    /// <summary>
    /// Speichert hochgeladene Bilder unter wwwroot/uploads.
    /// Aus Sicherheitsgründen sind nur Bildformate bis 5 MB erlaubt
    /// (sonst könnten z. B. HTML- oder JS-Dateien öffentlich ausgeliefert werden).
    /// </summary>
    public class PicSave
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif", ".avif" };
        private const long MaxBytes = 5 * 1024 * 1024;

        public static bool IsValidImage(IFormFile file, out string error)
        {
            error = null;
            if (file == null || file.Length == 0) return true;
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
            {
                error = "Nur Bilddateien (JPG, PNG, WEBP, GIF, AVIF) sind erlaubt.";
                return false;
            }
            if (file.Length > MaxBytes)
            {
                error = "Das Bild darf höchstens 5 MB groß sein.";
                return false;
            }
            if (!string.IsNullOrEmpty(file.ContentType) && !file.ContentType.StartsWith("image/"))
            {
                error = "Die Datei ist kein gültiges Bild.";
                return false;
            }
            return true;
        }

        /// <returns>Relativer Pfad (z. B. "/uploads/abc.jpg") oder null, wenn keine Datei übergeben wurde.</returns>
        public async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            if (!IsValidImage(file, out var error))
                throw new InvalidOperationException(error);

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueName = Guid.NewGuid() + Path.GetExtension(file.FileName).ToLowerInvariant();
            var filePath = Path.Combine(uploadsFolder, uniqueName);
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return "/uploads/" + uniqueName;
        }
    }
}

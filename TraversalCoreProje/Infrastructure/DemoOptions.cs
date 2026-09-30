namespace TraversalCoreProje.Infrastructure
{
    /// <summary>Einstellungen für den öffentlichen Demo-Modus (appsettings.json → "Demo").</summary>
    public class DemoOptions
    {
        /// <summary>Demo-Modus aktiv: Besucher können sich ohne Registrierung als Admin oder Mitglied anmelden.</summary>
        public bool Enabled { get; set; } = true;

        /// <summary>Datenbank beim Start der Anwendung auf den Ausgangszustand zurücksetzen.</summary>
        public bool ResetOnStartup { get; set; } = false;

        /// <summary>Intervall (Stunden), in dem die Demo-Datenbank automatisch zurückgesetzt wird. 0 = aus.</summary>
        public int ResetIntervalHours { get; set; } = 6;

        public string AdminEmail { get; set; } = "admin@traversal.demo";
        public string MemberEmail { get; set; } = "gast@traversal.demo";

        /// <summary>Passwort aller Demo-Konten (für die normale Anmeldung).</summary>
        public string DemoPassword { get; set; } = "Demo123!";

        /// <summary>Link zurück zum Portfolio (wird im Demo-Banner angezeigt).</summary>
        public string PortfolioUrl { get; set; } = "https://amirrezaafshar.de";

        /// <summary>Links zu Impressum und Datenschutzerklärung (Pflichtangaben in Deutschland).</summary>
        public string ImpressumUrl { get; set; } = "https://amirrezaafshar.de/Impressum";
        public string DatenschutzUrl { get; set; } = "https://amirrezaafshar.de/Datenschutz";

        /// <summary>Die Demo-Hauptkonten dürfen nicht gelöscht oder im Passwort geändert werden.</summary>
        public bool IsProtectedAccount(string email) =>
            Enabled && email != null &&
            (email.Equals(AdminEmail, StringComparison.OrdinalIgnoreCase) ||
             email.Equals(MemberEmail, StringComparison.OrdinalIgnoreCase));
    }
}

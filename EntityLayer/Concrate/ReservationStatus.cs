namespace EntityLayer.Concrate
{
    /// <summary>
    /// Zentrale Status-Texte für Reservierungen.
    /// Die Texte werden so in der Datenbank gespeichert (kompatibel mit den bisherigen Daten).
    /// </summary>
    public static class ReservationStatus
    {
        public const string Pending = "Ihre Genehmigung ist ausstehend.";
        public const string Approved = "Die Buchung ist bestätigt.";
        public const string Canceled = "Storniert";

        public static readonly string[] All = { Pending, Approved, Canceled };
    }
}

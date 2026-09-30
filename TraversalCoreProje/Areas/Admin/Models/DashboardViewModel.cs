using EntityLayer.Concrate;

namespace TraversalCoreProje.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int UserCount { get; set; }
        public int DestinationCount { get; set; }
        public int ActiveDestinationCount { get; set; }
        public int PendingCount { get; set; }
        public int ApprovedCount { get; set; }
        public int CanceledCount { get; set; }
        public int CommentCount { get; set; }
        public int GuideCount { get; set; }
        public int UnreadMessages { get; set; }
        public int NewsletterCount { get; set; }
        public double ExpectedRevenue { get; set; }

        /// <summary>Buchungen pro Monat (letzte 6 Monate) für das Diagramm.</summary>
        public List<string> MonthLabels { get; set; } = new();
        public List<int> MonthValues { get; set; } = new();

        /// <summary>Top-Reiseziele nach Anzahl Reservierungen.</summary>
        public List<(string City, int Count)> TopDestinations { get; set; } = new();

        public List<Reservition> LatestPending { get; set; } = new();
    }
}

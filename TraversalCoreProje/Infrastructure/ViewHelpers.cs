using System.Globalization;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Html;
using X.PagedList.Mvc.Core;

namespace TraversalCoreProje.Infrastructure
{
    /// <summary>Kleine Hilfsfunktionen für die Views (Formatierung, Status-Badges, Pager).</summary>
    public static class ViewHelpers
    {
        public static readonly CultureInfo De = new("de-DE");

        public static string Euro(double value) => value.ToString("#,##0", De) + " €";

        public static string Date(DateTime value) => value.ToString("dd.MM.yyyy", De);

        public static string LongDate(DateTime value) => value.ToString("dddd, d. MMMM yyyy", De);

        public static int Nights(Reservition r) => Math.Max(0, (r.ReservEnd.Date - r.ReservStart.Date).Days);

        public static IHtmlContent StatusBadge(string status)
        {
            var (css, icon, text) = status switch
            {
                ReservationStatus.Approved => ("badge-success", "fa-check", "Bestätigt"),
                ReservationStatus.Canceled => ("badge-danger", "fa-times", "Storniert"),
                _ => ("badge-warning", "fa-clock", "Ausstehend")
            };
            return new HtmlString($"<span class=\"badge {css} status-badge\"><i class=\"fas {icon} mr-1\"></i>{text}</span>");
        }

        public static IHtmlContent ActiveBadge(bool active, string on = "Aktiv", string off = "Inaktiv") =>
            new HtmlString(active
                ? $"<span class=\"badge badge-success status-badge\">{on}</span>"
                : $"<span class=\"badge badge-secondary status-badge\">{off}</span>");

        /// <summary>Einheitliche Bootstrap-4-Optik für X.PagedList.</summary>
        public static PagedListRenderOptions Pager => new()
        {
            UlElementClasses = new[] { "pagination", "justify-content-center", "mt-3" },
            LiElementClasses = new[] { "page-item" },
            PageClasses = new[] { "page-link" },
            LinkToPreviousPageFormat = "‹ Zurück",
            LinkToNextPageFormat = "Weiter ›",
            DisplayLinkToFirstPage = PagedListDisplayMode.Never,
            DisplayLinkToLastPage = PagedListDisplayMode.Never,
            DisplayLinkToPreviousPage = PagedListDisplayMode.IfNeeded,
            DisplayLinkToNextPage = PagedListDisplayMode.IfNeeded,
            MaximumPageNumbersToDisplay = 7
        };

        public static string Initials(string name) =>
            string.Concat((name ?? "?").Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(2).Select(p => char.ToUpper(p[0])));

        public static string Truncate(string text, int max) =>
            string.IsNullOrEmpty(text) || text.Length <= max ? text : text[..max].TrimEnd() + " …";
    }
}

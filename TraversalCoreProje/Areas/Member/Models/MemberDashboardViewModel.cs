using EntityLayer.Concrate;

namespace TraversalCoreProje.Areas.Member.Models
{
    public class MemberDashboardViewModel
    {
        public User User { get; set; }
        public int PendingCount { get; set; }
        public int ApprovedCount { get; set; }
        public int CanceledCount { get; set; }
        public int CommentCount { get; set; }
        public Reservition NextTrip { get; set; }
        public List<Reservition> Latest { get; set; } = new();
        public List<Destiniton> Suggestions { get; set; } = new();
    }
}

using EntityLayer.Concrate;

namespace TraversalCoreProje.Models
{
    public class DestinationListViewModel
    {
        public List<Destiniton> Destinations { get; set; } = new();
        public string Search { get; set; }
        public string Sort { get; set; }
    }

    public class DestinationDetailViewModel
    {
        public Destiniton Destination { get; set; }
        public Guide Guide { get; set; }
        public int CommentCount { get; set; }
        public List<Destiniton> Related { get; set; } = new();
    }

    public class AboutPageViewModel
    {
        public About About { get; set; }
        public List<SubAbout> SubAbouts { get; set; } = new();
        public List<Guide> Guides { get; set; } = new();
        public int DestinationCount { get; set; }
        public int CustomerCount { get; set; }
        public int ReservationCount { get; set; }
    }

    public class ContactPageViewModel
    {
        public Contact Contact { get; set; }
        public ContactMessage Message { get; set; } = new();
    }
}

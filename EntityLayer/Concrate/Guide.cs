using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrate
{
    /// <summary>Reiseführer (Guide), der Touren begleitet.</summary>
    public class Guide
    {
        [Key]
        public int GuideID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string TwitterUrl { get; set; }
        public string InstagramUrl { get; set; }
        public bool Status { get; set; }
        public List<Reservition> Reservations { get; set; }
    }
}

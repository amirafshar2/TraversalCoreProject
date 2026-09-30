using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrate
{
    public class Newsletter
    {
        [Key]
        public int NewsletterID { get; set; }
        public string Mail { get; set; }
        public DateTime SubscribeDate { get; set; } = DateTime.Now;
    }
}

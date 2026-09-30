using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrate
{
    /// <summary>Nachricht, die über das Kontaktformular der Website gesendet wurde.</summary>
    public class ContactMessage
    {
        [Key]
        public int ContactMessageID { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime MessageDate { get; set; }
        public bool IsRead { get; set; }
    }
}

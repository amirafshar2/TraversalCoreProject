using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrate
{
    public class Destiniton
    {
        [Key]
        public int DestinationID { get; set; }
        public string City { get; set; }
        public string DayNight { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public bool Status { get; set; }
        public string CoverImage { get; set; }
        public string Title1 { get; set; }
        public string Detail1 { get; set; }
        public string Detail2 { get; set; }
        public string Detail3 { get; set; }
        public string Title3 { get; set; }
        public string Detail4 { get; set; }
        public string Title4 { get; set; }
        public string Detail5 { get; set; }
        public string Title5 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Reservition> reservitions { get; set; }
        /// <summary>Id des Benutzers (Tourleiter/Admin), der das Reiseziel angelegt hat.</summary>
        public int Turlider { get; set; }
    }
}

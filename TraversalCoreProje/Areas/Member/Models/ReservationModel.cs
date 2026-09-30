using System.ComponentModel.DataAnnotations;

namespace TraversalCoreProje.Areas.Member.Models
{
    public class ReservationModel
    {
        public int id { get; set; }
        public int Destintionid { get; set; }

        [Range(1, 20, ErrorMessage = "Bitte wählen Sie 1 bis 20 Personen.")]
        public int HowmanyPapel { get; set; } = 2;

        [Required(ErrorMessage = "Bitte wählen Sie ein Anreisedatum.")]
        [DataType(DataType.Date)]
        public DateTime? ReservStart { get; set; }

        [Required(ErrorMessage = "Bitte wählen Sie ein Abreisedatum.")]
        [DataType(DataType.Date)]
        public DateTime? ReservEnd { get; set; }

        public int? Guidid { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        // Anzeige
        public string City { get; set; }
        public string DayNight { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
    }
}

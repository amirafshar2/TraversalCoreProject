using System.ComponentModel.DataAnnotations;

namespace TraversalCoreProje.Areas.Admin.Models
{
    public class DestinationModel
    {
        public int DestinationID { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie eine Stadt ein.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie die Dauer ein (z. B. 5 Tage / 4 Nächte).")]
        public string DayNight { get; set; }

        [Range(1, 100000, ErrorMessage = "Der Preis muss zwischen 1 und 100.000 € liegen.")]
        public double Price { get; set; }

        [Range(1, 500, ErrorMessage = "Die Kapazität muss zwischen 1 und 500 liegen.")]
        public int Capacity { get; set; } = 20;

        [Required(ErrorMessage = "Bitte geben Sie eine Kurzbeschreibung ein.")]
        public string Description { get; set; }

        public bool Status { get; set; } = true;

        public string Image { get; set; }
        public IFormFile image { get; set; }
        public string CoverImage { get; set; }
        public IFormFile coverImage { get; set; }
        public string Image2 { get; set; }
        public IFormFile image2 { get; set; }
        public string Image3 { get; set; }
        public IFormFile image3 { get; set; }

        public string Title1 { get; set; }
        public string Detail1 { get; set; }
        public string Detail2 { get; set; }
        public string Title3 { get; set; }
        public string Detail3 { get; set; }
        public string Title4 { get; set; }
        public string Detail4 { get; set; }
        public string Title5 { get; set; }
        public string Detail5 { get; set; }

        public int turliderid { get; set; }
        public string username { get; set; }
        public string usersurname { get; set; }
        public string userimage { get; set; }
        public int ReservationCount { get; set; }
    }
}

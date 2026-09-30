using EntityLayer.Concrate;
using TraversalCoreProje.Areas.Admin.Models;

namespace TraversalCoreProje.Areas.Admin.Mthods
{
    /// <summary>Umwandlung zwischen Entity (Destiniton) und ViewModel (DestinationModel).</summary>
    public class EntityTauchen
    {
        public Destiniton DestinationModelToDestiniton(DestinationModel p)
        {
            return new Destiniton
            {
                DestinationID = p.DestinationID,
                City = p.City,
                DayNight = p.DayNight,
                Price = p.Price,
                Capacity = p.Capacity,
                Image = p.Image,
                CoverImage = p.CoverImage,
                Image2 = p.Image2,
                Image3 = p.Image3,
                Description = p.Description,
                Status = p.Status,
                Title1 = p.Title1,
                Detail1 = p.Detail1,
                Detail2 = p.Detail2,
                Title3 = p.Title3,
                Detail3 = p.Detail3,
                Title4 = p.Title4,
                Detail4 = p.Detail4,
                Title5 = p.Title5,
                Detail5 = p.Detail5,
            };
        }

        public DestinationModel DestinitonToDestinationModel(Destiniton d)
        {
            return new DestinationModel
            {
                DestinationID = d.DestinationID,
                City = d.City,
                DayNight = d.DayNight,
                Price = d.Price,
                Capacity = d.Capacity,
                Image = d.Image,
                CoverImage = d.CoverImage,
                Image2 = d.Image2,
                Image3 = d.Image3,
                Description = d.Description,
                Status = d.Status,
                Title1 = d.Title1,
                Detail1 = d.Detail1,
                Detail2 = d.Detail2,
                Title3 = d.Title3,
                Detail3 = d.Detail3,
                Title4 = d.Title4,
                Detail4 = d.Detail4,
                Title5 = d.Title5,
                Detail5 = d.Detail5,
                turliderid = d.Turlider,
            };
        }
    }
}

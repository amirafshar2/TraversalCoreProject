using Microsoft.AspNetCore.Identity;

namespace EntityLayer.Concrate
{
    public class User : IdentityUser<int>
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string gender { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<Reservition> reservitions { get; set; }
    }
}

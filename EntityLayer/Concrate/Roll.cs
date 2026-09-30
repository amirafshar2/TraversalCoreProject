using Microsoft.AspNetCore.Identity;

namespace EntityLayer.Concrate
{
    public class Roll : IdentityRole<int>
    {
        public Roll() { }
        public Roll(string name) : base(name) { }
        public string Description { get; set; }
    }
}

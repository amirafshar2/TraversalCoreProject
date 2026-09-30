using System.ComponentModel.DataAnnotations;

namespace TraversalCoreProje.Areas.Admin.Models
{
    public class UserModel
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie einen Vornamen ein.")]
        public string name { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie einen Nachnamen ein.")]
        public string surename { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie eine E-Mail-Adresse ein.")]
        [EmailAddress(ErrorMessage = "Ungültige E-Mail-Adresse.")]
        public string email { get; set; }

        public string phone { get; set; }
        public string gender { get; set; }
        public string image { get; set; }
        public IFormFile ImageFile { get; set; }
        public List<string> Roles { get; set; } = new();
    }

    public class UserListItem
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public string Gender { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Roles { get; set; } = new();
        public int ReservationCount { get; set; }
        public int CommentCount { get; set; }
        public bool IsProtected { get; set; }
    }

    public class RoleAssignViewModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public List<RoleCheck> Roles { get; set; } = new();
    }

    public class RoleCheck
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Exists { get; set; }
    }

    public class RoleViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie einen Rollennamen ein.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Der Rollenname muss 3–30 Zeichen lang sein.")]
        public string Name { get; set; }

        public string Description { get; set; }
        public int UserCount { get; set; }
        public bool IsSystemRole { get; set; }
    }
}

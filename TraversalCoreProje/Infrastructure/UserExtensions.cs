using System.Security.Claims;

namespace TraversalCoreProje.Infrastructure
{
    public static class UserExtensions
    {
        public static bool IsAdmin(this ClaimsPrincipal user) => user?.IsInRole("Admin") == true;
        public static bool CanModerate(this ClaimsPrincipal user) => user?.IsInRole("Admin") == true || user?.IsInRole("Moderator") == true;
    }
}

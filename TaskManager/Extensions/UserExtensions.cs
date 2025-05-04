using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TaskManager.Models;


namespace TaskManager.Extensions
{
    public static class UserExtensions
    {
        // Both extensions used to check user roles since I didn't use EF for roles

        public static bool IsAdmin(this ClaimsPrincipal user, UserManager<User> userManager)
        {
            // Get user's role from the database and then check if it's Admin
            var dbUser = userManager.GetUserAsync(user).Result;
            return dbUser?.Role == RoleType.Admin;
        }

        public static bool IsManager(this ClaimsPrincipal user, UserManager<User> userManager)
        {
            // Get user's role from the database and then check if it's Manager
            var dbUser = userManager.GetUserAsync(user).Result;
            return dbUser?.Role == RoleType.Manager;
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabt.APIs.Extentions
{
    public static class UserManagerExtention
    {
        public static async Task<AppUser> FindUsersWithAddressAsync(this UserManager<AppUser> userManager,ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user=await userManager.Users.Include(u=>u.Address).SingleOrDefaultAsync(U=>U.Email==email);
            return user;
        }
    }
}

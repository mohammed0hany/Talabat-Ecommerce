using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Data;

namespace Talabat.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {
        public async static Task SeedUsersAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    DisplayName = "Ahmed Nasr",
                    Email = "ahmed.nasr@gmail.com",
                    UserName = "ahmed.nasr",
                    PhoneNumber = "01122335544"
                };
                await _userManager.CreateAsync(user ,"Pa$$w0rd");
            }
        }
    }
}

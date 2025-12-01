using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.IdentityData.DataSeed
{
    public class IdentityDataInatializer : IDataInatializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataInatializer> _logger;

        public IdentityDataInatializer(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager,ILogger<IdentityDataInatializer> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task InatializeAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if(!_userManager.Users.Any())
                {
                    var User01 = new ApplicationUser()
                    {
                        DisplayName = "Mohamed Tarek",
                        UserName = "MohamedTarek",
                        Email = "MohamedTarek@gmail.com",
                        PhoneNumber = "01236523652"
                    };
                    var User02 = new ApplicationUser()
                    {
                        DisplayName = "Ahmed Samy",
                        UserName = "AhmedSamy",
                        Email = "AhmedSamy@gmail.com",
                        PhoneNumber = "01016334658"
                    };

                    await _userManager.CreateAsync(User01,"P@ssw0rd");
                    await _userManager.CreateAsync(User02,"P@ssw0rd");

                    await _userManager.AddToRoleAsync(User01, "Admin");
                    await _userManager.AddToRoleAsync(User02, "SuperAdmin");
                }


            }
            catch(Exception ex)
            {
                _logger.LogError($"Error While Seeding Identity Database : Message = {ex.Message}");
            }
        }
    }
}

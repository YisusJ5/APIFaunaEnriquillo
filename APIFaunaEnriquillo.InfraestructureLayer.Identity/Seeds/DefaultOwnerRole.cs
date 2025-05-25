using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.DomainLayer.Enums;
using APIFaunaEnriquillo.InfraestructureLayer.Identity.Models;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;

namespace APIFaunaEnriquillo.InfraestructureLayer.Identity.Seeds
{
    public static class DefaultOwnerRole
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager) 
        {
            User ownerUser = new()
            {
                UserName = "Sr Waos",
                FirstName = "Waos",
                LastName = "o",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
        
            if (userManager.Users.All(i => i.Id != ownerUser.Id))
            {
                var user = await userManager.FindByEmailAsync(ownerUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(ownerUser, "FaunaEnriquillo");
                    await userManager.AddToRoleAsync(ownerUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(ownerUser, Roles.Editor.ToString());
                }

            }

        
        }

    }
}

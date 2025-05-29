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
    public static class DefaultOwnerRoles
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager) 
        {
            User ownerUser = new()
            {
                UserName = "Sr Waos",
                FirstName = "Admin",
                LastName = "Default",
                Email = "yaserviperez203@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
        
            if (userManager.Users.All(i => i.Id != ownerUser.Id))
            {
                var user = await userManager.FindByEmailAsync(ownerUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(ownerUser, "F4un4Enriquill0P4$s");
                    await userManager.AddToRoleAsync(ownerUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(ownerUser, Roles.Editor.ToString());
                    await userManager.AddToRoleAsync(ownerUser, Roles.Visitante.ToString());
                }

            }

        
        }

    }
}

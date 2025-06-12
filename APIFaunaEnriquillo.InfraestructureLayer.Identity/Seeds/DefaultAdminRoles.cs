using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.DomainLayer.Enums;
using APIFaunaEnriquillo.InfraestructureLayer.Identity.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace APIFaunaEnriquillo.InfraestructureLayer.Identity.Seeds
{
    public static class DefaultAdminRoles
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager) 
        {
            User ownerUser = new()
            {
                UserName = "SrEnrriquillo",
                FirstName = "Administrador",
                LastName = "Enriquillo",
                Email = "enrriquillowiki@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedAt  = DateTime.Now

            };

            var user = await userManager.FindByEmailAsync(ownerUser.Email);
            if (user == null)
            {
                var result = await userManager.CreateAsync(ownerUser, "F4un4Enriquill0P4$s#0");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(ownerUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(ownerUser, Roles.Editor.ToString());
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creando usuario seed: {error.Code} - {error.Description}");
                    }
                }
            }


        }

    }
}

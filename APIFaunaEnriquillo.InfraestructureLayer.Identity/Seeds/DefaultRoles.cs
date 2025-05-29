using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.DomainLayer.Enums;
using APIFaunaEnriquillo.InfraestructureLayer.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace APIFaunaEnriquillo.InfraestructureLayer.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Roles.Admin.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));

            if (!await roleManager.RoleExistsAsync(Roles.Editor.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Roles.Editor.ToString()));

            if (!await roleManager.RoleExistsAsync(Roles.Visitante.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Roles.Visitante.ToString()));
        }


    }
}

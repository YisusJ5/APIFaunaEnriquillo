using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.InfraestructureLayer.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace APIFaunaEnriquillo.InfraestructureLayer.Identity.Context
{
    public class IdentityContext : IdentityDbContext<User>
    {
        public IdentityContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("Identity");
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
            });

            modelBuilder.Entity<IdentityRole>(entity => {

                entity.ToTable("Role");

            });

            modelBuilder.Entity<IdentityUserRole<string>>(entity => {

                entity.ToTable("UserRole");

            });
            modelBuilder.Entity<IdentityUserLogin<string>>(entity => {

                entity.ToTable("UserLogin");

            });
        }





    }
}

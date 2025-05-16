using APIFaunaEnriquillo.Core.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.InfrastructureLayer.Persistence.Data
{
    public class FaunaDbContext: DbContext 
    {
        public FaunaDbContext(DbContextOptions<FaunaDbContext> options) : base(options) { }

        #region Modelos
        DbSet<Habitat> Habitats { get; set; }
        #endregion

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region Tables
            modelBuilder.Entity<Habitat>().ToTable("Habitats");
            #endregion

            #region Primary Key
            modelBuilder.Entity<Habitat>()
                .HasKey(k => k.IdHabitat)
                .HasName("PKHabitat");
            #endregion
        }
        #endregion
    }
}

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

        #region Models
        DbSet<Habitat> Habitats { get; set; }
        DbSet<Planta> Plantas { get; set; }
        DbSet<Animal> Animals { get; set; }
        #endregion

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region Tables
            modelBuilder.Entity<Habitat>().ToTable("Habitats");
            modelBuilder.Entity<Animal>().ToTable("Animales");
            modelBuilder.Entity<Planta>().ToTable("Plantas");
            #endregion

            #region Primary Key
            modelBuilder.Entity<Habitat>()
                .HasKey(k => k.IdHabitat)
                .HasName("PKHabitat");

            modelBuilder.Entity<Animal>().
                HasKey(k => k.IdAnimal).
                HasName("PKAnimal");

            modelBuilder.Entity<Planta>().
                HasKey(k => k.IdPlanta).
                HasName("PKPlanta");
            #endregion

            #region Properties
            modelBuilder.Entity<Habitat>(entity =>
            {
                entity.Property(h => h.IdHabitat).IsRequired();
                entity.Property(h => h.NombreComun).HasMaxLength(250);
                entity.Property(h => h.NombreCientifico).HasMaxLength(250);
                entity.Property(h => h.Descripcion).HasMaxLength(500);
                entity.Property(h => h.UbicacionGeograficaUrl).HasMaxLength(400);
                entity.Property(h => h.ImagenUrl).HasMaxLength(400);
            });
            modelBuilder.Entity<Animal>(entity =>
            {
                entity.Property(a => a.IdAnimal).IsRequired();
                entity.Property(a => a.HabitatId).IsRequired();
                entity.Property(a => a.NombreComun).HasMaxLength(250);
                entity.Property(a => a.NombreCientifico).HasMaxLength(250);
                entity.Property(a => a.Filo).HasMaxLength(250);
                entity.Property(a => a.Clase);
                entity.Property(a => a.Orden).HasMaxLength(200);
                entity.Property(a => a.Familia).HasMaxLength(200);
                entity.Property(a => a.Genero).HasMaxLength(100);
                entity.Property(a => a.Especie).HasMaxLength(100);
                entity.Property(a => a.SubEspecie).HasMaxLength(100);
                entity.Property(a => a.Observaciones).HasMaxLength(250);
                entity.Property(a => a.DistribucionGeograficaUrl).HasMaxLength(500);
                entity.Property(a => a.ImagenUrl).HasMaxLength(500);
            });

            modelBuilder.Entity<Planta>(entity =>
            {
                entity.Property(p => p.IdPlanta).IsRequired();
                entity.Property(p => p.HabitatId).IsRequired();
                entity.Property(p => p.NombreComun).HasMaxLength(250);
                entity.Property(p => p.NombreCientifico).HasMaxLength(250);
                entity.Property(p => p.Filo).HasMaxLength(200);
                entity.Property(p => p.Clase).HasMaxLength(200);
                entity.Property(p => p.Orden).HasMaxLength(200);
                entity.Property(p => p.Familia).HasMaxLength(200);
                entity.Property(p => p.Genero).HasMaxLength(200);
                entity.Property(p => p.Especie).HasMaxLength(200);
                entity.Property(p => p.SubEspecie).HasMaxLength(200);
                entity.Property(p => p.Observaciones).HasMaxLength(500);
                entity.Property(p => p.DistribucionGeograficaUrl).HasMaxLength(500);
                entity.Property(p => p.ImagenUrl).HasMaxLength(500);
            });
            #endregion

            #region Relationship
            modelBuilder.Entity<Planta>().
                HasOne(p => p.Habitat).
                WithMany(p => p.Plantas).
                HasForeignKey(p => p.HabitatId);

            modelBuilder.Entity<Animal>().
                HasOne(a => a.Habitat).
                WithMany(a => a.Animales).
                HasForeignKey(a => a.HabitatId);
            #endregion
        }
        #endregion


    }
}

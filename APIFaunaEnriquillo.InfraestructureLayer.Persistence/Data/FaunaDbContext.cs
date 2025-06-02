using APIFaunaEnriquillo.Core.DomainLayer.Agregate.HabitatAgregate;
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
                entity.OwnsOne(h => h.NombreComun, n =>
                {
                    n.Property(v => v.Value).HasColumnName("NombreComunHabitat").HasMaxLength(250);
                });
                entity.OwnsOne(h => h.NombreCientifico, n =>
                {
                    n.Property(v => v.Value).HasColumnName("NombreCientificoHabitat").HasMaxLength(250);
                });
                entity.Property(h => h.Descripcion).HasMaxLength(500);

            });
            modelBuilder.Entity<Animal>(entity =>
            {
                entity.Property(a => a.IdAnimal).IsRequired();
                entity.OwnsOne(a => a.NombreComun, n =>
                {
                    n.Property(p => p.Value).HasColumnName("NombreComunAnimal").HasMaxLength(250);
                });
                entity.OwnsOne(a => a.NombreCientifico, n =>
                {
                    n.Property(p => p.Value).HasColumnName("NombreCientificoAnimal").HasMaxLength(250);
                });
                entity.OwnsOne(a => a.Filo, n =>
                {
                    n.Property(p => p.Value).HasColumnName("FiloAnimal").HasMaxLength(250);
                });
                entity.OwnsOne(a => a.Clase, n =>
                {
                    n.Property(p => p.Value).HasColumnName("ClaseAnimal").HasMaxLength(250);
                });
                entity.OwnsOne(a => a.Orden, n =>
                {
                    n.Property(p => p.Value).HasColumnName("OrdenAnimal").HasMaxLength(200);
                });
                entity.OwnsOne(a => a.Familia, n =>
                {
                    n.Property(p => p.Value).HasColumnName("FamiliaAnimal").HasMaxLength(200);
                });
                entity.OwnsOne(a => a.Genero, n =>
                {
                    n.Property(p => p.Value).HasColumnName("GeneroAnimal").HasMaxLength(100);
                });
                entity.OwnsOne(a => a.Especie, n =>
                {
                    n.Property(p => p.Value).HasColumnName("EspecieAnimal").HasMaxLength(100);
                });
                entity.OwnsOne(a => a.SubEspecie, n =>
                {
                    n.Property(p => p.Value).HasColumnName("SubEspecieAnimal").HasMaxLength(100);
                });
                entity.Property(a => a.Observaciones).HasMaxLength(800);

            });

            modelBuilder.Entity<Planta>(entity =>
            {
                entity.Property(p => p.IdPlanta).IsRequired();
                entity.OwnsOne(p => p.NombreComun, n =>
                {
                    n.Property(v => v.Value).HasColumnName("NombreComunPlanta").HasMaxLength(250);
                });
                entity.OwnsOne(p => p.NombreCientifico, n =>
                {
                    n.Property(v => v.Value).HasColumnName("NombreCientificoPlanta").HasMaxLength(250);
                });
                entity.OwnsOne(p => p.Filo, n =>
                {
                    n.Property(v => v.Value).HasColumnName("FiloPlanta").HasMaxLength(200);
                });
                entity.OwnsOne(p => p.Clase, n =>
                {
                    n.Property(v => v.Value).HasColumnName("ClasePlanta").HasMaxLength(200);
                });
                entity.OwnsOne(p => p.Orden, n =>
                {
                    n.Property(v => v.Value).HasColumnName("OrdenPlanta").HasMaxLength(200);
                });
                entity.OwnsOne(p => p.Familia, n =>
                {
                    n.Property(v => v.Value).HasColumnName("FamiliaPlanta").HasMaxLength(200);
                });
                entity.OwnsOne(p => p.Genero, n =>
                {
                    n.Property(v => v.Value).HasColumnName("GeneroPlanta").HasMaxLength(200);
                });
                entity.OwnsOne(p => p.Especie, n =>
                {
                    n.Property(v => v.Value).HasColumnName("EspeciePlanta").HasMaxLength(200);
                });
                entity.OwnsOne(p => p.SubEspecie, n =>
                {
                    n.Property(v => v.Value).HasColumnName("SubEspeciePlanta").HasMaxLength(200);
                });
                entity.Property(p => p.Observaciones).HasMaxLength(800);

            });
            #endregion

            #region Relationship
            modelBuilder.Entity<Planta>().
                HasOne(p => p.Habitat).
                WithMany(p => p.Plantas).
                HasForeignKey(p => p.HabitatId).
                OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Animal>().
                HasOne(a => a.Habitat).
                WithMany(a => a.Animales).
                HasForeignKey(a => a.HabitatId).
                OnDelete(DeleteBehavior.SetNull);
            #endregion
        }
        #endregion


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoftComputacion.Domain;
using Microsoft.EntityFrameworkCore;

namespace LoftComputacion.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


       
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<MetodoDePago> MetodosDePago { get; set; }
        public DbSet<OrdenDeServicio> OrdenesDeServicio { get; set; }   
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Foto> Fotos { get; set; }
        public DbSet<HistorialOrden> HistorialOrdenes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuramos la precisión para las propiedades de tipo decimal
            modelBuilder.Entity<OrdenDeServicio>(entity =>
            {
                entity.Property(e => e.PrecioFinal).HasPrecision(18, 2);
                entity.Property(e => e.PrecioPresupuestado).HasPrecision(18, 2);
            });

            
            // Configuramos la entidad Cliente para que el DNI sea único
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasIndex(e => e.DNI).IsUnique();
            });
            
        }
    }


}
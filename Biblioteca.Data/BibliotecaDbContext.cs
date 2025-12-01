using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Biblioteca.Data
{
    public class BibliotecaDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public BibliotecaDbContext(DbContextOptions<BibliotecaDbContext> options) : base(options)
        {
        }

        public BibliotecaDbContext()
        {
        }

        public DbSet<Libro> Libros { get; set; }
        public DbSet<Socio> Socios { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BibliotecaDB;Trusted_Connection=true;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // IMPORTANTE para Identity

            // Tus configuraciones existentes
            modelBuilder.Entity<Libro>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(l => l.Autor).IsRequired().HasMaxLength(100);
                entity.Property(l => l.ISBN).HasMaxLength(20);
                entity.Property(l => l.Genero).HasMaxLength(50);
            });

            modelBuilder.Entity<Socio>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.CodigoSocio).IsRequired().HasMaxLength(20);
                entity.Property(s => s.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Email).HasMaxLength(100);
                entity.HasIndex(s => s.CodigoSocio).IsUnique();
            });

            modelBuilder.Entity<Prestamo>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.CodigoPrestamo).IsRequired().HasMaxLength(20);
                entity.Property(p => p.Estado).IsRequired().HasMaxLength(20);
                entity.HasIndex(p => p.CodigoPrestamo).IsUnique();
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(u => u.Email).IsUnique();
            });
        }
    }
}
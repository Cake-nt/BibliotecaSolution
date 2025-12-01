using Microsoft.EntityFrameworkCore;
using Biblioteca.Data;
using System;
using System.Threading.Tasks;

namespace Biblioteca.Web.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<BibliotecaDbContext>();

            // SOLO datos de biblioteca - SIN Identity por ahora
            await PoblarDatosBibliotecaAsync(context);
        }

        private static async Task PoblarDatosBibliotecaAsync(BibliotecaDbContext context)
        {
            if (!await context.Categorias.AnyAsync())
            {
                // Agregar categorías
                var categorias = new[]
                {
                    new Biblioteca.Core.Models.Categoria { Nombre = "Ficción", Descripcion = "Libros de ficción", Activo = true },
                    new Biblioteca.Core.Models.Categoria { Nombre = "No Ficción", Descripcion = "Libros de no ficción", Activo = true },
                    new Biblioteca.Core.Models.Categoria { Nombre = "Científico", Descripcion = "Libros científicos", Activo = true },
                    new Biblioteca.Core.Models.Categoria { Nombre = "Educativo", Descripcion = "Libros educativos", Activo = true }
                };

                await context.Categorias.AddRangeAsync(categorias);
                await context.SaveChangesAsync();

                // Agregar algunos libros
                var categoria = categorias[0];
                var libros = new[]
                {
                    new Biblioteca.Core.Models.Libro
                    {
                        Titulo = "Cien años de soledad",
                        Autor = "Gabriel García Márquez",
                        ISBN = "978-8437604947",
                        Genero = "Realismo Mágico",
                        AnioPublicacion = 1967,
                        Editorial = "Sudamericana",
                        EjemplaresTotales = 5,
                        EjemplaresDisponibles = 5,
                        Activo = true,
                        CategoriaId = categoria.Id
                    }
                };

                await context.Libros.AddRangeAsync(libros);
                await context.SaveChangesAsync();
            }
        }
    }
}
using Biblioteca.Business.Interfaces;
using Biblioteca.Business.Services;
using Biblioteca.Core.Models;
using Biblioteca.Data;
using Biblioteca.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace Biblioteca.WindowsForms1
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        [STAThread]
        static async Task Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Inicializar base de datos
            using (var context = new BibliotecaDbContext())
            {
                // await context.InitializeAsync();
            }

            // Configurar Dependency Injection simple
            var services = new ServiceCollection();

            services.AddScoped<BibliotecaDbContext>();
            services.AddScoped<ILibroRepository, LibroRepository>();
            services.AddScoped<ISocioRepository, SocioRepository>();
            services.AddScoped<IPrestamoRepository, PrestamoRepository>();
            services.AddScoped<LibroService>();
            services.AddScoped<SocioService>();
            services.AddScoped<PrestamoService>();

            services.AddTransient<MainForm>();
            services.AddTransient<frmLibros>();
            services.AddTransient<frmSocios>();
            services.AddTransient<frmPrestamos>();

            var serviceProvider = services.BuildServiceProvider();
            Program.ServiceProvider = serviceProvider;

            Application.Run(serviceProvider.GetService<MainForm>());
        }

        static void ConfigureServices(ServiceCollection services)
        {
            // Configurar DbContext
            services.AddDbContext<BibliotecaDbContext>(options =>
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BibliotecaDB;Trusted_Connection=true;"));

            // Repositories
            services.AddScoped<ILibroRepository, LibroRepository>();
            services.AddScoped<ISocioRepository, SocioRepository>();
            services.AddScoped<IPrestamoRepository, PrestamoRepository>();
            services.AddScoped<IRepository<Categoria>, Repository<Categoria>>();

            // Services
            services.AddScoped<LibroService>();
            services.AddScoped<SocioService>();
            services.AddScoped<PrestamoService>();
            services.AddScoped<CategoriaService>();

            // Forms
            services.AddTransient<MainForm>();
            services.AddTransient<frmLibros>();
            services.AddTransient<frmSocios>();
            services.AddTransient<frmPrestamos>();
        }

        static async Task InicializarBaseDeDatosAsync()
        {
            using var scope = ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BibliotecaDbContext>();

            // Crear la base de datos si no existe
            await context.Database.EnsureCreatedAsync();

            // Poblar con datos iniciales
            await PoblarDatosInicialesAsync(context);
        }

        static async Task PoblarDatosInicialesAsync(BibliotecaDbContext context)
        {
            // Verificar si ya hay datos
            if (!await context.Categorias.AnyAsync())
            {
                // Agregar categorías
                var categorias = new[]
                {
                    new Biblioteca.Core.Models.Categoria { Nombre = "Ficción", Descripcion = "Libros de ficción" },
                    new Biblioteca.Core.Models.Categoria { Nombre = "No Ficción", Descripcion = "Libros de no ficción" },
                    new Biblioteca.Core.Models.Categoria { Nombre = "Científico", Descripcion = "Libros científicos" },
                    new Biblioteca.Core.Models.Categoria { Nombre = "Educativo", Descripcion = "Libros educativos" }
                };
                await context.Categorias.AddRangeAsync(categorias);
                await context.SaveChangesAsync();

                // Agregar algunos libros de ejemplo
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
                        CategoriaId = categoria.Id
                    },
                    new Biblioteca.Core.Models.Libro
                    {
                        Titulo = "1984",
                        Autor = "George Orwell",
                        ISBN = "978-0451524935",
                        Genero = "Ciencia Ficción",
                        AnioPublicacion = 1949,
                        Editorial = "Secker & Warburg",
                        EjemplaresTotales = 3,
                        EjemplaresDisponibles = 3,
                        CategoriaId = categoria.Id
                    }
                };
                await context.Libros.AddRangeAsync(libros);
                await context.SaveChangesAsync();

                // Agregar un socio de ejemplo
                var socio = new Biblioteca.Core.Models.Socio
                {
                    CodigoSocio = "SOC-20241201-0001",
                    Nombre = "Juan",
                    Apellido = "Pérez",
                    Email = "juan@email.com",
                    Telefono = "123456789"
                };
                await context.Socios.AddAsync(socio);
                await context.SaveChangesAsync();
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Biblioteca.Core.Models;
using Biblioteca.Business.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Data.Repositories
{
    public class LibroRepository : Repository<Libro>, ILibroRepository
    {
        public LibroRepository(BibliotecaDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Libro>> GetLibrosDisponiblesAsync()
        {
            return await _dbSet
                .Where(l => l.EjemplaresDisponibles > 0 && l.Activo)
                .Include(l => l.Categoria)
                .ToListAsync();
        }

        public async Task<IEnumerable<Libro>> SearchLibrosAsync(string criterio)
        {
            return await _dbSet
                .Where(l => l.Activo &&
                           (l.Titulo.Contains(criterio) ||
                            l.Autor.Contains(criterio) ||
                            l.Genero.Contains(criterio) ||
                            l.ISBN.Contains(criterio)))
                .Include(l => l.Categoria)
                .ToListAsync();
        }

        public async Task<Libro> GetByISBNAsync(string isbn)
        {
            return await _dbSet
                .FirstOrDefaultAsync(l => l.ISBN == isbn && l.Activo);
        }

        // Override para incluir categoría en las búsquedas
        public override async Task<Libro> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public override async Task<IEnumerable<Libro>> GetAllAsync()
        {
            return await _dbSet
                .Where(l => l.Activo)
                .Include(l => l.Categoria)
                .ToListAsync();
        }
    }
}
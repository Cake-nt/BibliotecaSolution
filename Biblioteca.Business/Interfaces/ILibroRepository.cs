using Biblioteca.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Biblioteca.Business.Interfaces
{
    public interface ILibroRepository : IRepository<Libro>
    {
        Task<IEnumerable<Libro>> GetLibrosDisponiblesAsync();
        Task<IEnumerable<Libro>> SearchLibrosAsync(string criterio);
        Task<Libro> GetByISBNAsync(string isbn);
    }
}
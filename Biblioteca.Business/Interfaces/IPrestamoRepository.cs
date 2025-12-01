using Biblioteca.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Biblioteca.Business.Interfaces
{
    public interface IPrestamoRepository : IRepository<Prestamo>
    {
        Task<IEnumerable<Prestamo>> GetPrestamosActivosAsync();
        Task<IEnumerable<Prestamo>> GetPrestamosVencidosAsync();
        Task<IEnumerable<Prestamo>> GetPrestamosBySocioAsync(int socioId);
        Task<Prestamo> GetByCodigoAsync(string codigoPrestamo);
    }
}
using Biblioteca.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Biblioteca.Business.Interfaces
{
    public interface ISocioRepository : IRepository<Socio>
    {
        Task<Socio> GetByCodigoAsync(string codigoSocio);
        Task<IEnumerable<Socio>> GetSociosActivosAsync();
        Task<bool> SocioTienePrestamosActivosAsync(int socioId);
    }
}
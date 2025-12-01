using Microsoft.EntityFrameworkCore;
using Biblioteca.Core.Models;
using Biblioteca.Business.Interfaces;
using Biblioteca.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Data.Repositories
{
    public class SocioRepository : Repository<Socio>, ISocioRepository
    {
        public SocioRepository(BibliotecaDbContext context) : base(context)
        {
        }

        public async Task<Socio> GetByCodigoAsync(string codigoSocio)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.CodigoSocio == codigoSocio && s.Activo);
        }

        public async Task<IEnumerable<Socio>> GetSociosActivosAsync()
        {
            return await _dbSet
                .Where(s => s.Activo)
                .ToListAsync();
        }

        public async Task<bool> SocioTienePrestamosActivosAsync(int socioId)
        {
            return await _context.Prestamos
                .AnyAsync(p => p.SocioId == socioId && p.Estado == "Activo");
        }
    }
}
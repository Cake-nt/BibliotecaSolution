using Microsoft.EntityFrameworkCore;
using Biblioteca.Core.Models;
using Biblioteca.Business.Interfaces;
using Biblioteca.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Data.Repositories
{
    public class PrestamoRepository : Repository<Prestamo>, IPrestamoRepository
    {
        public PrestamoRepository(BibliotecaDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Prestamo>> GetPrestamosActivosAsync()
        {
            return await _dbSet
                .Where(p => p.Estado == "Activo")
                .Include(p => p.Libro)
                .Include(p => p.Socio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prestamo>> GetPrestamosVencidosAsync()
        {
            var hoy = DateTime.Today;
            return await _dbSet
                .Where(p => p.Estado == "Activo" && p.FechaDevolucionPrevista < hoy)
                .Include(p => p.Libro)
                .Include(p => p.Socio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prestamo>> GetPrestamosBySocioAsync(int socioId)
        {
            return await _dbSet
                .Where(p => p.SocioId == socioId)
                .Include(p => p.Libro)
                .OrderByDescending(p => p.FechaPrestamo)
                .ToListAsync();
        }

        public async Task<Prestamo> GetByCodigoAsync(string codigoPrestamo)
        {
            return await _dbSet
                .Include(p => p.Libro)
                .Include(p => p.Socio)
                .FirstOrDefaultAsync(p => p.CodigoPrestamo == codigoPrestamo);
        }

        // Override para incluir relaciones en las búsquedas
        public override async Task<Prestamo> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Libro)
                .Include(p => p.Socio)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<IEnumerable<Prestamo>> GetAllAsync()
        {
            return await _dbSet
                .Include(p => p.Libro)
                .Include(p => p.Socio)
                .OrderByDescending(p => p.FechaPrestamo)
                .ToListAsync();
        }
    }
}
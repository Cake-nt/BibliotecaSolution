using Biblioteca.Core.Models;
using Biblioteca.Business.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Biblioteca.Business.Services
{
    public class SocioService
    {
        private readonly ISocioRepository _socioRepository;
        private readonly IPrestamoRepository _prestamoRepository;

        public SocioService(ISocioRepository socioRepository, IPrestamoRepository prestamoRepository)
        {
            _socioRepository = socioRepository;
            _prestamoRepository = prestamoRepository;
        }

        public async Task<IEnumerable<Socio>> GetAllSociosAsync()
        {
            return await _socioRepository.GetAllAsync();
        }

        public async Task<Socio> GetSocioByIdAsync(int id)
        {
            return await _socioRepository.GetByIdAsync(id);
        }

        public async Task<Socio> CreateSocioAsync(Socio socio)
        {
            // Validar que el código de socio sea único
            var socioExistente = await _socioRepository.GetByCodigoAsync(socio.CodigoSocio);
            if (socioExistente != null)
                throw new ArgumentException("Ya existe un socio con ese código");

            await _socioRepository.AddAsync(socio);
            await _socioRepository.SaveAsync();
            return socio;
        }

        public async Task UpdateSocioAsync(Socio socio)
        {
            _socioRepository.Update(socio);
            await _socioRepository.SaveAsync();
        }

        public async Task DeleteSocioAsync(int id)
        {
            var socio = await _socioRepository.GetByIdAsync(id);
            if (socio != null)
            {
                // Verificar que no tenga préstamos activos
                bool tienePrestamos = await _socioRepository.SocioTienePrestamosActivosAsync(id);
                if (tienePrestamos)
                    throw new InvalidOperationException("No se puede eliminar el socio porque tiene préstamos activos");

                // Eliminación lógica
                socio.Activo = false;
                _socioRepository.Update(socio);
                await _socioRepository.SaveAsync();
            }
        }

        public async Task<IEnumerable<Socio>> GetSociosActivosAsync()
        {
            return await _socioRepository.GetSociosActivosAsync();
        }

        public async Task<bool> SocioPuedePedirPrestamoAsync(int socioId)
        {
            var socio = await _socioRepository.GetByIdAsync(socioId);
            return socio != null && socio.Activo;
        }
    }
}
using Biblioteca.Core.Models;
using Biblioteca.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Biblioteca.Business.Services
{
    public class PrestamoService
    {
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly ILibroRepository _libroRepository;
        private readonly ISocioRepository _socioRepository;

        public PrestamoService(
            IPrestamoRepository prestamoRepository,
            ILibroRepository libroRepository,
            ISocioRepository socioRepository)
        {
            _prestamoRepository = prestamoRepository;
            _libroRepository = libroRepository;
            _socioRepository = socioRepository;
        }

        public async Task<IEnumerable<Prestamo>> GetAllPrestamosAsync()
        {
            return await _prestamoRepository.GetAllAsync();
        }

        public async Task<Prestamo> GetPrestamoByIdAsync(int id)
        {
            return await _prestamoRepository.GetByIdAsync(id);
        }

        public async Task<Prestamo> CreatePrestamoAsync(Prestamo prestamo)
        {
            // Validar que el libro exista y tenga ejemplares disponibles
            var libro = await _libroRepository.GetByIdAsync(prestamo.LibroId);
            if (libro == null || !libro.Activo)
                throw new ArgumentException("El libro no existe o no está activo");

            if (libro.EjemplaresDisponibles <= 0)
                throw new InvalidOperationException("No hay ejemplares disponibles de este libro");

            // Validar que el socio exista y esté activo
            var socio = await _socioRepository.GetByIdAsync(prestamo.SocioId);
            if (socio == null || !socio.Activo)
                throw new ArgumentException("El socio no existe o no está activo");

            // Validar que el socio no tenga muchos préstamos activos (opcional)
            var prestamosActivos = await _prestamoRepository.GetPrestamosBySocioAsync(prestamo.SocioId);
            // Puedes agregar un límite de préstamos por socio aquí

            // Configurar datos del préstamo
            prestamo.FechaPrestamo = DateTime.Now;
            prestamo.FechaDevolucionPrevista = DateTime.Now.AddDays(15); // 15 días por defecto
            prestamo.Estado = "Activo";
            prestamo.CodigoPrestamo = GenerateCodigoPrestamo();

            // Actualizar disponibilidad del libro
            libro.EjemplaresDisponibles--;
            _libroRepository.Update(libro);

            await _prestamoRepository.AddAsync(prestamo);
            await _prestamoRepository.SaveAsync();

            return prestamo;
        }

        public async Task DevolverPrestamoAsync(int prestamoId)
        {
            var prestamo = await _prestamoRepository.GetByIdAsync(prestamoId);
            if (prestamo == null)
                throw new ArgumentException("Préstamo no encontrado");

            if (prestamo.Estado == "Devuelto")
                throw new InvalidOperationException("El préstamo ya fue devuelto");

            // Actualizar estado del préstamo
            prestamo.Estado = "Devuelto";
            prestamo.FechaDevolucionReal = DateTime.Now;

            // Actualizar disponibilidad del libro
            var libro = await _libroRepository.GetByIdAsync(prestamo.LibroId);
            if (libro != null)
            {
                libro.EjemplaresDisponibles++;
                _libroRepository.Update(libro);
            }

            _prestamoRepository.Update(prestamo);
            await _prestamoRepository.SaveAsync();
        }

        public async Task<IEnumerable<Prestamo>> GetPrestamosActivosAsync()
        {
            return await _prestamoRepository.GetPrestamosActivosAsync();
        }

        public async Task<IEnumerable<Prestamo>> GetPrestamosVencidosAsync()
        {
            return await _prestamoRepository.GetPrestamosVencidosAsync();
        }

        public async Task<IEnumerable<Prestamo>> GetPrestamosBySocioAsync(int socioId)
        {
            return await _prestamoRepository.GetPrestamosBySocioAsync(socioId);
        }

        private string GenerateCodigoPrestamo()
        {
            return $"PRE-{DateTime.Now:yyyyMMdd-HHmmss}";
        }
    }
}
using Biblioteca.Core.Models;
using Biblioteca.Business.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Biblioteca.Business.Services
{
    public class LibroService
    {
        private readonly ILibroRepository _libroRepository;

        public LibroService(ILibroRepository libroRepository)
        {
            _libroRepository = libroRepository;
        }

        // AGREGAR ESTE MÉTODO QUE FALTA:
        public async Task<Libro> CreateLibroAsync(Libro libro)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(libro.Titulo))
                throw new System.ArgumentException("El título es requerido");

            if (string.IsNullOrWhiteSpace(libro.Autor))
                throw new System.ArgumentException("El autor es requerido");

            if (libro.EjemplaresTotales < 1)
                throw new System.ArgumentException("Debe haber al menos 1 ejemplar total");

            if (libro.EjemplaresDisponibles < 0)
                throw new System.ArgumentException("Los ejemplares disponibles no pueden ser negativos");

            if (libro.EjemplaresDisponibles > libro.EjemplaresTotales)
                throw new System.ArgumentException("Los ejemplares disponibles no pueden ser mayores a los totales");

            // Asignar valores por defecto
            libro.Activo = true;

            await _libroRepository.AddAsync(libro);
            await _libroRepository.SaveAsync();
            return libro;
        }

        // Los otros métodos que ya tienes:
        public async Task<IEnumerable<Libro>> GetAllLibrosAsync()
        {
            return await _libroRepository.GetAllAsync();
        }

        public async Task<Libro> GetLibroByIdAsync(int id)
        {
            return await _libroRepository.GetByIdAsync(id);
        }

        public async Task UpdateLibroAsync(Libro libro)
        {
            _libroRepository.Update(libro);
            await _libroRepository.SaveAsync();
        }

        public async Task DeleteLibroAsync(int id)
        {
            var libro = await _libroRepository.GetByIdAsync(id);
            if (libro != null)
            {
                libro.Activo = false;
                _libroRepository.Update(libro);
                await _libroRepository.SaveAsync();
            }
        }

        public async Task<IEnumerable<Libro>> GetLibrosDisponiblesAsync()
        {
            return await _libroRepository.GetLibrosDisponiblesAsync();
        }

        public async Task<IEnumerable<Libro>> SearchLibrosAsync(string criterio)
        {
            return await _libroRepository.SearchLibrosAsync(criterio);
        }

        public async Task<Libro> GetByISBNAsync(string isbn)
        {
            return await _libroRepository.GetByISBNAsync(isbn);
        }
    }
}
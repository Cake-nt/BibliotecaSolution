using Biblioteca.Core.Models;
using Biblioteca.Business.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Biblioteca.Business.Services
{
    public class CategoriaService
    {
        private readonly IRepository<Categoria> _categoriaRepository;

        public CategoriaService(IRepository<Categoria> categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<Categoria>> GetAllCategoriasAsync()
        {
            return await _categoriaRepository.GetAllAsync();
        }

        public async Task<Categoria> GetCategoriaByIdAsync(int id)
        {
            return await _categoriaRepository.GetByIdAsync(id);
        }

        public async Task<Categoria> CreateCategoriaAsync(Categoria categoria)
        {
            await _categoriaRepository.AddAsync(categoria);
            await _categoriaRepository.SaveAsync();
            return categoria;
        }

        public async Task UpdateCategoriaAsync(Categoria categoria)
        {
            _categoriaRepository.Update(categoria);
            await _categoriaRepository.SaveAsync();
        }

        public async Task DeleteCategoriaAsync(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria != null)
            {
                // Validar que no tenga libros asociados (aquí necesitarías una validación adicional)
                categoria.Activo = false;
                _categoriaRepository.Update(categoria);
                await _categoriaRepository.SaveAsync();
            }
        }
    }
}
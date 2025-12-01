using System.Collections.Generic;

namespace Biblioteca.Core.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
        public string Genero { get; set; }
        public int AnioPublicacion { get; set; }
        public string Editorial { get; set; }
        public int EjemplaresDisponibles { get; set; }
        public int EjemplaresTotales { get; set; }
        public bool Activo { get; set; } = true;
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
        public ICollection<Prestamo> Prestamos { get; set; }
    }
}
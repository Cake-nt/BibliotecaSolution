using System.Collections.Generic;

namespace Biblioteca.Core.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; } = true;
        public ICollection<Libro> Libros { get; set; }
    }
}
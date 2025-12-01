using System;

namespace Biblioteca.Core.Models
{
    public class Prestamo
    {
        public int Id { get; set; }
        public string CodigoPrestamo { get; set; }
        public DateTime FechaPrestamo { get; set; } = DateTime.Now;
        public DateTime FechaDevolucionPrevista { get; set; }
        public DateTime? FechaDevolucionReal { get; set; }
        public string Estado { get; set; }
        public int LibroId { get; set; }
        public Libro Libro { get; set; }
        public int SocioId { get; set; }
        public Socio Socio { get; set; }
    }
}
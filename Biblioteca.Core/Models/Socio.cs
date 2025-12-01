using System;
using System.Collections.Generic;

namespace Biblioteca.Core.Models
{
    public class Socio
    {
        public int Id { get; set; }
        public string CodigoSocio { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Activo { get; set; } = true;

        public ICollection<Prestamo> Prestamos { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}
using System;

namespace Biblioteca.Core.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Rol { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public int? SocioId { get; set; }
        public Socio Socio { get; set; }
    }
}
using LogicaDeNegocios.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public bool Suscrito { get; set; }
        public DateTime FechaSuscripcion { get; set; }
        public List<EnumDivisa> DivisasNotificaciones { get; set; } = new List<EnumDivisa>();
        public string SubscriptionPaypalId { get; set; }
        public bool AceptaTerminos { get; set; }
        public UsuarioDto() { }

        public UsuarioDto(Usuario u)
        {
            if (u == null) throw new ArgumentNullException(nameof(u), "El usuario no puede ser null.");
            Id = u.Id;
            Nombre = u.Nombre;
            Email = u.Email;
            Telefono = u.Telefono;
            Suscrito = u.Suscrito;
            FechaSuscripcion = u.FechaSuscripcion;
            DivisasNotificaciones = u.DivisasNotificaciones;
            SubscriptionPaypalId = u.SubscriptionPayPalId;
            AceptaTerminos = u.AceptaTerminos;
        }
        public Usuario ToUsuario()
        {
            Usuario usuario = new Usuario()
            {
                Id = Id,
                Nombre = Nombre,
                Email = Email,
                Telefono = Telefono,
                Suscrito = Suscrito,
                FechaSuscripcion = FechaSuscripcion,
                DivisasNotificaciones = DivisasNotificaciones,
                SubscriptionPayPalId = SubscriptionPaypalId,
                AceptaTerminos = AceptaTerminos
            };
            return usuario;
        }
    }
}

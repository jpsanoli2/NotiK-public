using LogicaDeNegocios.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class UsuarioAltaDto
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Solo aquí se maneja la contraseña
        public bool AceptaTerminos { get; set; }

        public UsuarioAltaDto() { }
        public UsuarioAltaDto(string nombre, string email, string password, bool aceptaTerminos)
        {
            if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("The name cannot be empty.");
            else if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("The email cannot be empty.");
            else if (!new EmailAddressAttribute().IsValid(email)) throw new ArgumentException("The email is not valid.");
            else if (password.Length < 6) throw new ArgumentException("The password must contain at least 6 characters.");
            else if (aceptaTerminos == false) throw new ArgumentException("You must accept the terms and conditions.");
            Nombre = nombre;
            Email = email;
            Password = password;
            AceptaTerminos = aceptaTerminos;
        }

        public Usuario ToUsuario()
        {
            return new Usuario(Nombre, Email, Password, AceptaTerminos); // Se encripta en el constructor de Usuario
        }
    }

}

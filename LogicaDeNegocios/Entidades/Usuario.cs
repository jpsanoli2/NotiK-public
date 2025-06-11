using LogicaDeNegocios.Excepciones;
using LogicaDeNegocios.InterfacesEntidades;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using BCrypt.Net;


namespace LogicaDeNegocios.Entidades
{
    public class Usuario : IIdentify
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The name is required.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "The email is required.")]
        [EmailAddress(ErrorMessage = "The email is not valid.")]
        public string Email { get; set; }
        public string PassHash { get; private set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public bool Suscrito { get; set; } = false;
        public DateTime FechaSuscripcion { get; set; } = DateTime.MinValue;
        public bool EmailVerificado { get; set; } = false;
        public string? TokenVerificacion {  get; set; } = null;
        public string? TokenRecuperacion { get; set; } = null;
        public List<EnumDivisa> DivisasNotificaciones { get; set; } = new List<EnumDivisa>();
        public List<EnumRiesgo> RiesgoNotificaciones { get; set; } = new List<EnumRiesgo>();
        public string? SubscriptionPayPalId { get; set; } = null;
        public bool AceptaTerminos { get; set; }


        public Usuario() { }
        public Usuario(string nombre, string email, string password, bool aceptaTerminos)
        {
            if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("Name cannot be empty.");
            else if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be empty.");
            else if (!new EmailAddressAttribute().IsValid(email)) throw new ArgumentException("Email is not valid");
            else if (password.Length < 6) throw new ArgumentException("Password must contain at least 6 characters.");
            else if (aceptaTerminos != true) throw new ArgumentException("You must accept the terms and conditions.");
            Nombre = nombre;
            Email = email;
            PassHash = HashPassword(password);
            AceptaTerminos = aceptaTerminos;
        }
        public void Suscribir()
        {
            Suscrito = true;
        }
        public void CancelarSuscripcion()
        {
            Suscrito = false;
        }
        public void CambiarPass(string newPass)
        {
            if (string.IsNullOrWhiteSpace(newPass)) throw new ArgumentException("New password cannot be empty.");
            PassHash = HashPassword(newPass);
        }
        public bool VerificarPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PassHash);
        }
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }
        public void verificarTelefono(string telefono)
        {
            Telefono = telefono;
        }
        public void Copiar(Usuario u)
        {
            Nombre = u.Nombre;
            Email = u.Email;
            PassHash = u.PassHash;
            Telefono = u.Telefono;
            Suscrito = u.Suscrito;
            FechaSuscripcion = u.FechaSuscripcion;
            AceptaTerminos = u.AceptaTerminos;
        }
        public void Validar()
        {
            var context = new ValidationContext(this, null, null);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(this, context, results, true))
            {
                throw new ValidationException(results.First().ErrorMessage);
            }
        }
    }
}

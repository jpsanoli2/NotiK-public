using Dto;
using LogicaDeAplicacion.InterfacesCU.InterfacesEmail;
using LogicaDeAplicacion.InterfacesCU.InterfacesUsuario;
using LogicaDeNegocios.Entidades;
using LogicaDeNegocios.Excepciones;
using LogicaDeNegocios.InterfacesRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LogicaDeAplicacion.ImplementacionCU.ImplementacionUsuario
{
    public class AltaUsuario: IAltaUsuario
    {
        private readonly IRepositorioUsuario _repositorio;
        private readonly IEnviarEmail _enviarEmail;

        public AltaUsuario(IRepositorioUsuario repositorio, IEnviarEmail enviarEmail)
        {
            _repositorio = repositorio;
            _enviarEmail = enviarEmail;
        }

        public async Task Ejecutar(UsuarioAltaDto uDto)
        {
            Usuario usuario = new Usuario(uDto.Nombre, uDto.Email, uDto.Password, uDto.AceptaTerminos);
            usuario.TokenVerificacion = Guid.NewGuid().ToString();
            usuario.Validar();
            Usuario u = await _repositorio.Alta(usuario);
            if (u != null)
            {

                string linkVerificacion = $"https://localhost:44350/Usuario/VerificarCuenta?token={usuario.TokenVerificacion}";

                _enviarEmail.Ejecutar(usuario.Email, "Verify your account.", $@"
            <!DOCTYPE html>
            <html>
                <head>
                    <meta charset='UTF-8'>
                    <title>Account Verification</title>
                </head>
                <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; text-align: center;'>
                    <div style='max-width: 500px; margin: auto; background: white; padding: 20px; border-radius: 10px; box-shadow: 0px 0px 10px rgba(0,0,0,0.1);'>
                        <h2 style='color: #333;'>¡Welcome to our platform.!</h2>
                        <p style='color: #555;'>Thank you for signing up. To complete the process, verify your account by clicking the button below:</p>
                        <a href='{linkVerificacion}' 
                        style='display: inline-block; background-color: #28a745; color: white; padding: 12px 20px; text-decoration: none; 
                        border-radius: 5px; font-weight: bold; margin-top: 10px;'>
                        Verify Account
                        </a>
                        <p style='color: #777; font-size: 14px; margin-top: 20px;'>If you did not request this verification, please ignore this message.</p>
                    </div>
                </body>
            </html>
            ");
            }
            else {
                throw new NotValidException("This user is already registered.");
            }
        }
    }
}

using Dto;
using LogicaDeAplicacion.InterfacesCU.InterfacesEmail;
using LogicaDeAplicacion.InterfacesCU.InterfacesUsuario;
using LogicaDeNegocios.Entidades;
using LogicaDeNegocios.InterfacesRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeAplicacion.ImplementacionCU.ImplementacionUsuario
{
    public class RecuperarPass : IRecuperarPass
    {
        private IRepositorioUsuario _repositorio;
        private readonly IEnviarEmail _enviarEmail;
        public RecuperarPass(IRepositorioUsuario repositorio, IEnviarEmail enviarEmail)
        {
            _repositorio = repositorio;
            _enviarEmail = enviarEmail;
        }
        public void Ejecutar(UsuarioDto uDto)
        {
            Usuario usuario = uDto.ToUsuario();
            _repositorio.RecuperarPass(usuario);
            string linkVerificacion = $"https://localhost:7169/Usuario/RecuperarPass?token={usuario.TokenRecuperacion}";
            _enviarEmail.Ejecutar(usuario.Email, "Recover your password.", $@"
            <!DOCTYPE html>
            <html>
                <head>
                    <meta charset='UTF-8'>
                    <title>Password recovery.</title>
                </head>
                <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; text-align: center;'>
                    <div style='max-width: 500px; margin: auto; background: white; padding: 20px; border-radius: 10px; box-shadow: 0px 0px 10px rgba(0,0,0,0.1);'>
                        <h2 style='color: #333;'>Password recovery</h2>
                        <p style='color: #555;'>Recover your account by clicking the button below.:</p>
                        <a href='{linkVerificacion}' 
                        style='display: inline-block; background-color: #28a745; color: white; padding: 12px 20px; text-decoration: none; 
                        border-radius: 5px; font-weight: bold; margin-top: 10px;'>
                        Verify Account
                        </a>
                        <p style='color: #777; font-size: 14px; margin-top: 20px;'>If you did not request this recovery, please ignore this message.</p>
                    </div>
                </body>
            </html>
            ");
        }
    }
}

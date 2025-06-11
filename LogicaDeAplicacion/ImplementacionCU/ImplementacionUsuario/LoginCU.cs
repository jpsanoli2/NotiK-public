using Dto;
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
    public class LoginCU: ILogin
    {
        private IRepositorioUsuario _repositorioUsuario;
        public LoginCU(IRepositorioUsuario repositorioUsuario)
        {
            _repositorioUsuario = repositorioUsuario;
        }
        public async Task<UsuarioDto> Ejecutar(string email, string pass)
        {
            Usuario usuario = await _repositorioUsuario.Login(email, pass);
            return new UsuarioDto(usuario);
        }
    }
}

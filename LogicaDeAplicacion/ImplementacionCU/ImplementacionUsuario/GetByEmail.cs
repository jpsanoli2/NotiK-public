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
    public class GetByEmail: IGetByEmail
    {
        private IRepositorioUsuario _repositorio;
        public GetByEmail(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<UsuarioDto> Ejecutar(string email)
        {
            Usuario usuarioObtenido = await _repositorio.GetByEmail(email);
            if(usuarioObtenido != null)
            {
                UsuarioDto usuarioDto = new UsuarioDto(usuarioObtenido);
                return usuarioDto;
            }
            return null;
        }
    }
}


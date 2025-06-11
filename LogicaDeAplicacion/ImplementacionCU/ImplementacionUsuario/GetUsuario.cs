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
    public class GetUsuario : IGetUsuario
    {
        private IRepositorioUsuario _repositorio;
        public GetUsuario(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<UsuarioDto> Ejecutar(int id)
        {
            Usuario usuarioObtenido = await _repositorio.Get(id);
            UsuarioDto usuarioDto = new UsuarioDto(usuarioObtenido);
            if(usuarioDto != null)
            {
                return usuarioDto;
            }
            return null;
        }
    }
}

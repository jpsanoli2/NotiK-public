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
    public class ModificarUsuario: IModificarUsuario
    {
        private IRepositorioUsuario _repositorio;
        public ModificarUsuario(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<UsuarioDto> Ejecutar(int id, UsuarioDto usuarioDto)
        {
            Usuario usuario = usuarioDto.ToUsuario();
            usuario.Validar();
            Usuario usuarioModificado = await _repositorio.Modificar(id, usuario);
            UsuarioDto usuarioModificadoDto = new UsuarioDto(usuarioModificado);
            return usuarioModificadoDto;
        }
    }
}

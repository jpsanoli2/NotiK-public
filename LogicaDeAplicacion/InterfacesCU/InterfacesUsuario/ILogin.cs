using Dto;
using LogicaDeNegocios.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeAplicacion.InterfacesCU.InterfacesUsuario
{
    public interface ILogin
    {
        Task<UsuarioDto> Ejecutar(string email, string pass);
    }
}

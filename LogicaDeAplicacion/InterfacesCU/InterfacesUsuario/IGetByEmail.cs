using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeAplicacion.InterfacesCU.InterfacesUsuario
{
    public interface IGetByEmail
    {
        Task<UsuarioDto> Ejecutar(string email);
    }
}

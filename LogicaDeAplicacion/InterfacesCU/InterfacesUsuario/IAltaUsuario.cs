using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeAplicacion.InterfacesCU.InterfacesUsuario
{
    public interface IAltaUsuario
    {
        Task Ejecutar(UsuarioAltaDto uDto);
    }
}

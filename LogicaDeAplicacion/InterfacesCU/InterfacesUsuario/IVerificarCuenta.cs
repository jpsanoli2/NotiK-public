using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeAplicacion.InterfacesCU.InterfacesUsuario
{
    public interface IVerificarCuenta
    {
        Task<bool> Ejecutar(string token);
    }
}

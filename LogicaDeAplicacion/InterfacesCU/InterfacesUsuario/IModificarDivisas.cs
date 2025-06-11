using LogicaDeNegocios.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeAplicacion.InterfacesCU.InterfacesUsuario
{
    public interface IModificarDivisas
    {
        Task Ejecutar(List<EnumDivisa> divisas, int id);
    }
}

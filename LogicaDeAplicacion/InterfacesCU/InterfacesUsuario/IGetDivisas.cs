using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaDeNegocios.Entidades;

namespace LogicaDeAplicacion.InterfacesCU.InterfacesUsuario
{
    public interface IGetDivisas
    {
        Task<IEnumerable<EnumDivisa>> Ejecutar(int id);
    }
}

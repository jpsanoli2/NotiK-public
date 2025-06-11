using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaDeNegocios.Entidades;

namespace LogicaDeAplicacion.InterfacesCU.InterfacesUsuario
{
    public interface IModificarRiesgos
    {
        Task Ejecutar(List<EnumRiesgo> divisas, int id);
    }
}

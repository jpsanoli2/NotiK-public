using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeAplicacion.InterfacesCU.InterfacesEventosEconomicos
{
    public interface IGetAllEventoEconomico
    {
        Task<IEnumerable<EventoEconomicoDto>> Ejecutar();
    }
}

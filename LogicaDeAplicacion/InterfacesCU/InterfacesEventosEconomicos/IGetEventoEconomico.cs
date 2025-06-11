using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dto;

namespace LogicaDeAplicacion.InterfacesCU.InterfacesEventosEconomicos
{
    public interface IGetEventoEconomico
    {
        Task<EventoEconomicoDto> Ejecutar(int id);
    }
}

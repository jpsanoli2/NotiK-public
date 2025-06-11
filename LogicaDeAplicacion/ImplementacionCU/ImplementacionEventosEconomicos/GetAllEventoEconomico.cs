using Dto;
using LogicaDeAplicacion.InterfacesCU.InterfacesEventosEconomicos;
using LogicaDeNegocios.InterfacesRepositorios;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeAplicacion.ImplementacionCU.ImplementacionEventosEconomicos
{
    public class GetAllEventoEconomico : IGetAllEventoEconomico
    {
        private IRepositorioEventoEconomico _repositorio;

        public GetAllEventoEconomico(IRepositorioEventoEconomico repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IEnumerable<EventoEconomicoDto>> Ejecutar()
        {
            List<EventoEconomicoDto> eventosDto = new List<EventoEconomicoDto>();
            IEnumerable<EventoEconomico> eventos = await _repositorio.GetAll();
            foreach(EventoEconomico evento in eventos)
            {
                eventosDto.Add(new EventoEconomicoDto(evento));
            }
            return eventosDto;
        }
    }
}

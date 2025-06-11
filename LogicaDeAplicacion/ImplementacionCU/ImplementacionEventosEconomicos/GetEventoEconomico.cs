using Dto;
using LogicaDeAplicacion.InterfacesCU.InterfacesEventosEconomicos;
using LogicaDeNegocios.InterfacesRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeAplicacion.ImplementacionCU.ImplementacionEventosEconomicos
{
    public class GetEventoEconomico: IGetEventoEconomico
    {
        private IRepositorioEventoEconomico _repositorio;

        public GetEventoEconomico(IRepositorioEventoEconomico repositorio)
        {
            _repositorio = repositorio;
        }
        public async Task<EventoEconomicoDto> Ejecutar(int id)
        {
            EventoEconomico eventoObtenido = await _repositorio.Get(id);
            EventoEconomicoDto eventoDto = new EventoEconomicoDto(eventoObtenido);
            return eventoDto;
        }
    }
}

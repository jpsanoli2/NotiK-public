using LogicaDeNegocios.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class EventoEconomicoDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public EnumDivisa Divisa { get; set; }
        public EnumRiesgo Riesgo { get; set; }

        public EventoEconomicoDto() { }
        public EventoEconomicoDto(EventoEconomico e)
        {
            Nombre = e.Nombre;
            Descripcion = e.Descripcion;
            Divisa = e.Divisa;
            Riesgo = e.Riesgo;
        }
        public EventoEconomico ToEventoEconomico()
        {
            EventoEconomico eventoEconomico = new EventoEconomico() 
            { 
                Id = Id,
                Fecha = Fecha,
                Nombre = Nombre,
                Descripcion = Descripcion,
                Divisa = Divisa,
                Riesgo = Riesgo
            };
            return eventoEconomico;
        }
    }
}

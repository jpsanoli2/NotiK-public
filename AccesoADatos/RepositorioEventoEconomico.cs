using LogicaDeNegocios.Entidades;
using LogicaDeNegocios.InterfacesRepositorios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoADatos
{
    public class RepositorioEventoEconomico : RepositorioGenerico<EventoEconomico>, IRepositorioEventoEconomico
    {
        private DbContext _context;
        public RepositorioEventoEconomico(DbContext contexto) : base(contexto)
        {
            _context = contexto;
        }

        public async Task<IEnumerable<EventoEconomico>> GetEventosDelDia(DateTime fecha)
        {
            var eventos = await _context.Set<EventoEconomico>()
                                  .Where(e => e.Fecha.Date == fecha.Date)
                                  .ToListAsync();
            return eventos;
        }
    }
}

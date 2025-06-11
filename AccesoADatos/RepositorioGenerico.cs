using LogicaDeNegocios.Excepciones;
using LogicaDeNegocios.InterfacesEntidades;
using LogicaDeNegocios.InterfacesRepositorios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoADatos
{
    public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T: class, IIdentify
    {
        protected readonly DbContext _context;
       
        public RepositorioGenerico(DbContext contexto)
        {
            _context = contexto;

        }
        public T Alta(T t)
        {
            _context.Set<T>().Add(t);
            _context.SaveChanges();
            return t;
        }

        public void Eliminar(int Id)
        {
            T tAEliminar = _context.Set<T>()
                     .FirstOrDefault(t => t.Id == Id);
            if (tAEliminar != null)
            {
                _context.Set<T>().Remove(tAEliminar);
                _context.SaveChanges();
            }
            else
            {
                throw new NotFoundException("Not found");
            }
        }

        public async Task<T> Get(int Id)
        {
            return await _context.Set<T>()
                     .FirstOrDefaultAsync(t => t.Id == Id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }


    }
}

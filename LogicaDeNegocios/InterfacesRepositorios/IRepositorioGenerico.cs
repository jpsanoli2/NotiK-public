using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeNegocios.InterfacesRepositorios
{
    public interface IRepositorioGenerico<T> where T : class
    {
        T Alta(T t);
        Task<T> Get(int Id);
        void Eliminar(int Id);
        Task<IEnumerable<T>> GetAll();
    }
}

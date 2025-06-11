using LogicaDeAplicacion.InterfacesCU.InterfacesUsuario;
using LogicaDeNegocios.InterfacesRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeAplicacion.ImplementacionCU.ImplementacionUsuario
{
    public class EliminarUsuario: IEliminarUsuario
    {
        private IRepositorioUsuario _repositorio;
        public EliminarUsuario(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }

        public void Ejecutar(int id)
        {
            _repositorio.Eliminar(id);
        }
    }
}

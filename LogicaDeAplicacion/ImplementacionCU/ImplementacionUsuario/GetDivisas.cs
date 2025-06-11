using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaDeAplicacion.InterfacesCU.InterfacesUsuario;
using LogicaDeNegocios.Entidades;
using LogicaDeNegocios.InterfacesRepositorios;

namespace LogicaDeAplicacion.ImplementacionCU.ImplementacionUsuario
{
    public class GetDivisas : IGetDivisas
    {
        private IRepositorioUsuario _repositorio;
        public GetDivisas(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }
        public async Task<IEnumerable<EnumDivisa>> Ejecutar(int id)
        {
            return await _repositorio.GetDivisas(id);
        }
    }
}

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
    public class GetRiesgos : IGetRiesgos
    {
        private IRepositorioUsuario _repositorio;
        public GetRiesgos(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }
        public async Task<IEnumerable<EnumRiesgo>> Ejecutar(int id)
        {
            return await _repositorio.GetRiesgos(id);
        }
    }
}

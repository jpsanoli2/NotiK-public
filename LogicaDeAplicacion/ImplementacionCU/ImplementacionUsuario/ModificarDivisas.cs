using LogicaDeAplicacion.InterfacesCU.InterfacesUsuario;
using LogicaDeNegocios.Entidades;
using LogicaDeNegocios.InterfacesRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeAplicacion.ImplementacionCU.ImplementacionUsuario
{
    public class ModificarDivisas : IModificarDivisas
    {
        private IRepositorioUsuario _repositorio;
        public ModificarDivisas(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }
        public async Task Ejecutar(List<EnumDivisa> divisas, int id)
        {
            await _repositorio.ModificarDivisas(divisas, id);
        }
    }
}

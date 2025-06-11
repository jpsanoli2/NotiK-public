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
    public class ModificarRiesgos: IModificarRiesgos
    {
        private IRepositorioUsuario _repositorio;
        public ModificarRiesgos(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }
        public async Task Ejecutar(List<EnumRiesgo> riesgos, int id)
        {
            await _repositorio.ModificarRiesgos(riesgos, id);
        }
    }
}


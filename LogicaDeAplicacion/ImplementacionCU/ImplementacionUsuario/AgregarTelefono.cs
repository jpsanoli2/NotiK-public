using Dto;
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
    public class AgregarTelefono : IAgregarTelefono
    {
        private IRepositorioUsuario _repositorio;
        public AgregarTelefono(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }
        public async Task Ejecutar(string telefono, int id)
        {
            await _repositorio.AgregarTelefono(telefono, id);   
        }
    }
}

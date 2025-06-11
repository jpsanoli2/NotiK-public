using LogicaDeAplicacion.InterfacesCU.InterfacesUsuario;
using LogicaDeNegocios.InterfacesRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeAplicacion.ImplementacionCU.ImplementacionUsuario
{
    public class VerificarUsuario : IVerificarCuenta
    {
        private IRepositorioUsuario _repositorio;
        public VerificarUsuario(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }
        public async Task<bool> Ejecutar(string token)
        {
            return await _repositorio.VerificarCuenta(token);
        }
    }
}

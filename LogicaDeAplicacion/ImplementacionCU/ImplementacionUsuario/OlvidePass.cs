using LogicaDeAplicacion.InterfacesCU.InterfacesUsuario;
using LogicaDeNegocios.InterfacesRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeAplicacion.ImplementacionCU.ImplementacionUsuario
{
    public class OlvidePass : IOlvidePass
    {
        private IRepositorioUsuario _repositorio;
        public OlvidePass(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }
        public void Ejecutar(string token, string pass)
        {
            if (string.IsNullOrWhiteSpace(pass) || pass.Length < 6)
            {
                throw new ArgumentException("The password must be at least 6 characters long.");
            }
            _repositorio.OlvidePass(token, pass);
        }
    }
}

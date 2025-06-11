using LogicaDeAplicacion.InterfacesCU.InterfacesEmail;
using LogicaDeAplicacion.InterfacesCU.InterfacesUsuario;
using LogicaDeNegocios.InterfacesRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeAplicacion.ImplementacionCU.ImplementacionUsuario
{
    public class CambiarPass : ICambiarPass
    {
        private readonly IRepositorioUsuario _repositorio;

        public CambiarPass(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }
        public void Ejecutar(string pass, int id)
        {
            if (string.IsNullOrWhiteSpace(pass) || pass.Length < 6)
            {
                throw new ArgumentException("The password must be at least 6 characters long.");
            }
            _repositorio.CambiarPass(pass, id);
        }
    }
}

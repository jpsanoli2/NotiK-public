using LogicaDeNegocios.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeNegocios.InterfacesRepositorios
{
    public interface IRepositorioUsuario: IRepositorioGenerico<Usuario>
    {
        Task<Usuario> Modificar(int id, Usuario u);
        Task<Usuario> Login(string email, string pass);
        Task<Usuario> Alta(Usuario usuario);
        Task<bool> VerificarCuenta(string token);
        Task AgregarTelefono(string telefono, int id);
        Task CambiarPass(string pass, int id);
        Task RecuperarPass(Usuario usuario);
        Task<Usuario> GetByEmail(string email);
        Task OlvidePass(string token, string pass);
        Task ModificarDivisas(List<EnumDivisa> divisas, int id);
        Task ModificarRiesgos(List<EnumRiesgo> riesgos, int id);
        Task<IEnumerable<Usuario>> GetAllSuscriptores();
        Task<Usuario> GetBySubId(string subscriptionId);
        Task<IEnumerable<EnumDivisa>> GetDivisas(int id);
        Task<IEnumerable<EnumRiesgo>> GetRiesgos(int id);

    }
}

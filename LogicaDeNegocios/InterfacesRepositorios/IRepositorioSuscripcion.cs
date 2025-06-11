using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeNegocios.InterfacesRepositorios
{
    public interface IRepositorioSuscripcion
    {
        Task ActivarSuscripcion(string subscriptionId);
        Task CancelarSuscripcion(string subscriptionId);
        Task RegistrarPago(string subscriptionId);
        Task MarcarPagoFallido(string subscriptionId);
        Task GuardadSubscriptionId(int usuarioId, string subscriptionId);
        Task<string> ObtenerSubscriptionId(int usuarioId);
    }
}

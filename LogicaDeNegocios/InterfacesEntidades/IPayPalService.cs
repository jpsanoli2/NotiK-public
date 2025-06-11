using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeNegocios.InterfacesEntidades
{
    public interface IPayPalService
    {
        Task<string> ObtenerTokenAcceso();
        Task<Payment> CrearPago(decimal monto, string descripcion, string returnUrl, string cancelUrl);
        Task<Payment> EjecutarPago(string paymentId, string payerId);
        
    }
}

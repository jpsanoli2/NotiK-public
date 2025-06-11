using LogicaDeNegocios.InterfacesRepositorios;
using Microsoft.AspNetCore.Mvc;
using LogicaDeNegocios.Entidades;
using LogicaDeNegocios.InterfacesEntidades;
using System.Text;
using System.Text.Json;

namespace Noticias.Controllers
{
    public class PagosVistaController : Controller
    {
        private readonly IRepositorioSuscripcion _suscripcionService;
        private readonly IPayPalService _payPalService;
        public PagosVistaController(IRepositorioSuscripcion suscripcionService, IPayPalService payPalService)
        {
            _suscripcionService = suscripcionService;
            _payPalService = payPalService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GuardarSubscriptionId([FromBody] string subscriptionId)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId is null) return Unauthorized();

            await _suscripcionService.GuardadSubscriptionId(usuarioId.Value, subscriptionId);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> CancelarSuscripcion()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return Unauthorized();

            var subscriptionId = await _suscripcionService.ObtenerSubscriptionId(usuarioId.Value);
            if (subscriptionId == null) return NotFound();

            var accessToken = await _payPalService.ObtenerTokenAcceso();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var contenidoCancelacion = new StringContent(
                JsonSerializer.Serialize(new { reason = "The user canceled from the application." }),
                Encoding.UTF8,
                "application/json"
            );

            var result = await client.PostAsync(
                $"https://api-m.sandbox.paypal.com/v1/billing/subscriptions/{subscriptionId}/cancel",
                contenidoCancelacion
            );

            if (result.IsSuccessStatusCode)
            {
                await _suscripcionService.CancelarSuscripcion(subscriptionId);
                TempData["mensajeExito"] = "Subscription canceled successfully.";
            }
            else
            {
                var error = await result.Content.ReadAsStringAsync();
                ViewBag.Error = $"Error canceling the subscription.: {error}";
            }

            return RedirectToAction("Index");
        }


    }
}

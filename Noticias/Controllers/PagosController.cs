using LogicaDeNegocios.InterfacesEntidades;
using LogicaDeNegocios.InterfacesRepositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using PayPal.Api;
using System.Text;

namespace Noticias.Controllers
{
    [ApiController]
    [Route("api/webhook/paypal")]
    public class PagosController : ControllerBase
    {
        private readonly IRepositorioSuscripcion _suscripcionService;
        private readonly ILogger<PagosController> _logger;

        public PagosController(IRepositorioSuscripcion suscripcionService, ILogger<PagosController> logger)
        {
            _suscripcionService = suscripcionService;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> Receive()
        {
            try
            {
                using var reader = new StreamReader(Request.Body, Encoding.UTF8);
                var body = await reader.ReadToEndAsync();
                var json = JObject.Parse(body);
                int usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;

                string? eventType = json["event_type"]?.ToString();
                string? subscriptionId = json["resource"]?["id"]?.ToString();

                _logger.LogInformation($"Evento recibido: {eventType}, SubscriptionId: {subscriptionId}");

                if (string.IsNullOrEmpty(subscriptionId))
                {
                    _logger.LogWarning("Subscription ID vacío");
                    return BadRequest();
                }

                // Manejo de eventos de PayPal
                switch (eventType)
                {
                    case "BILLING.SUBSCRIPTION.CREATED":
                        await _suscripcionService.GuardadSubscriptionId(usuarioId, subscriptionId); // Solo si tenés una forma de mapear el usuario
                        break;

                    case "BILLING.SUBSCRIPTION.ACTIVATED":
                        await _suscripcionService.ActivarSuscripcion(subscriptionId);
                        break;

                    case "BILLING.SUBSCRIPTION.CANCELLED":
                        await _suscripcionService.CancelarSuscripcion(subscriptionId);
                        break;

                    case "PAYMENT.SALE.COMPLETED":
                        await _suscripcionService.RegistrarPago(subscriptionId);
                        break;

                    case "PAYMENT.SALE.DENIED":
                        await _suscripcionService.MarcarPagoFallido(subscriptionId);
                        break;

                    default:
                        _logger.LogInformation($"Evento no manejado: {eventType}");
                        break;
                }

                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error processing PayPal webhook.");
                return StatusCode(500);
            }
        }
        
    }
}

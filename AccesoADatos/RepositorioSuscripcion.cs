using LogicaDeNegocios.Entidades;
using LogicaDeNegocios.Excepciones;
using LogicaDeNegocios.InterfacesEntidades;
using LogicaDeNegocios.InterfacesRepositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AccesoADatos
{
    public class RepositorioSuscripcion: IRepositorioSuscripcion
    {
        private readonly DbContext _context;
        private readonly ILogger<RepositorioSuscripcion> _logger;
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IPayPalService _payPalService;
        public RepositorioSuscripcion(DbContext contexto, ILogger<RepositorioSuscripcion> logger, IRepositorioUsuario repositorioUsuario, IPayPalService payPalService)
        {
            _context = contexto;
            _logger = logger;
            _repositorioUsuario = repositorioUsuario;
            _payPalService = payPalService;
        }

        public async Task ActivarSuscripcion(string subscriptionId)
        {
            Usuario usuario = await _repositorioUsuario.GetBySubId(subscriptionId);
            if(usuario == null)
            {
                throw new NotFoundException("User not found.");
            }
            usuario.Suscrito = true;
            usuario.FechaSuscripcion = DateTime.UtcNow;
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task CancelarSuscripcion(string subscriptionId)
        {
            Usuario usuario = await _repositorioUsuario.GetBySubId(subscriptionId);
            if(usuario == null)
            {
                throw new NotFoundException("User not found.");
            }
            usuario.Suscrito = false;
            usuario.SubscriptionPayPalId = null;
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<string> ObtenerSubscriptionId(int usuarioId)
        {
            Usuario usuario = await _repositorioUsuario.Get(usuarioId);
            return usuario.SubscriptionPayPalId;
        }

        public async Task GuardadSubscriptionId(int usuarioId, string subscriptionId)
        {
            Usuario usuario = await _repositorioUsuario.Get(usuarioId);
            if (usuario == null)
                throw new NotFoundException("User not found.");

            usuario.SubscriptionPayPalId = subscriptionId;

            // Verificamos en PayPal si está activa
            var estado = await ObtenerEstadoDeSuscripcion(subscriptionId);

            if (estado == "ACTIVE")
                usuario.Suscrito = true;

            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }


        public async Task MarcarPagoFallido(string subscriptionId)
        {
            Usuario usuario = await _repositorioUsuario.GetBySubId(subscriptionId);
            if(usuario == null)
            {
                throw new NotFoundException("User not found.");
            }
            usuario.Suscrito = false;
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RegistrarPago(string subscriptionId)
        {
            Usuario usuario = await _repositorioUsuario.GetBySubId(subscriptionId);
            if(usuario == null)
            {
                throw new NotFoundException("User not found.");
            }
            usuario.FechaSuscripcion = DateTime.UtcNow;
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        private async Task<string> ObtenerEstadoDeSuscripcion(string subscriptionId)
        {
            var accessToken = await _payPalService.ObtenerTokenAcceso();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"https://api-m.sandbox.paypal.com/v1/billing/subscriptions/{subscriptionId}");
            response.EnsureSuccessStatusCode();

            var contenido = await response.Content.ReadFromJsonAsync<PayPalSubscriptionResponse>();
            return contenido?.status;
        }

    }
}

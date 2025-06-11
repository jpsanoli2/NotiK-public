using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Types;
using Twilio;
using LogicaDeNegocios.InterfacesRepositorios;
using LogicaDeNegocios.Entidades;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Exceptions;
using OpenQA.Selenium;
using Microsoft.Extensions.Options;

namespace LogicaDeAplicacion.ImplementacionCU.ImplementacionMensajes
{
    public class mensajeriawhatsappbackgroundservice : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _horaProgramada = TimeSpan.Zero;
        //TimeSpan.Zero; // 00:00 UTC
        private IRepositorioUsuario _repositorio;
        private IRepositorioEventoEconomico _repositorioEvento;
        private readonly TwilioOptions _twilioOptions;
        public mensajeriawhatsappbackgroundservice(
        IServiceProvider serviceProvider,
        IOptions<TwilioOptions> twilioOptions)
        {
            _serviceProvider = serviceProvider;
            _twilioOptions = twilioOptions.Value;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var ahora = DateTime.UtcNow;
                var proximaEjecucion = DateTime.UtcNow.Date.Add(_horaProgramada);

                if (ahora > proximaEjecucion)
                    proximaEjecucion = proximaEjecucion.AddDays(1);

                var espera = proximaEjecucion - ahora;
                await Task.Delay(espera, stoppingToken);

                var dia = DateTime.UtcNow.DayOfWeek;
                if (dia >= DayOfWeek.Monday && dia <= DayOfWeek.Friday)
                {
                    await EnviarMensajeAasync();
                }

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }

        private async Task EnviarMensajeAasync()
        {
            using var scope = _serviceProvider.CreateScope();
            var repositorioUsuarios = scope.ServiceProvider.GetRequiredService<IRepositorioUsuario>();
            var repositorioEventos = scope.ServiceProvider.GetRequiredService<IRepositorioEventoEconomico>();

            var usuarios = await repositorioUsuarios.GetAllSuscriptores();
            var eventos = await repositorioEventos.GetEventosDelDia(DateTime.UtcNow); // hora local del servidor

            if (!eventos.Any())
                return;
            
            TwilioClient.Init(_twilioOptions.AccountSid, _twilioOptions.AuthToken);

            foreach (var usuario in usuarios)
            {
                var mensaje = $"Hello {usuario.Nombre}, These are today’s economic events. 📊:\n\n";

                foreach (var evento in eventos)
                {
                    if (usuario.DivisasNotificaciones.Contains(evento.Divisa) && usuario.RiesgoNotificaciones.Contains(evento.Riesgo))
                    {
                        mensaje += $"- {evento.Nombre} at {evento.Fecha:HH:mm}, risk {evento.Riesgo}";
                    }
                }

                try
                {
                    await MessageResource.CreateAsync(
                        from: new PhoneNumber("whatsapp:+14155238886"),
                        to: new PhoneNumber($"whatsapp:{usuario.Telefono}"),
                        body: mensaje
                    );
                }
                catch (ApiException ex)
                {
                    Console.WriteLine($"Error sending message: {ex.Message}");
                }

                await Task.Delay(500);
            }
        }

    }
}

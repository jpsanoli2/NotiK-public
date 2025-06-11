using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaDeNegocios.InterfacesEntidades;
using OpenQA.Selenium;
using PayPal.Api;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace LogicaDeNegocios.Entidades
{
    public class PayPalService: IPayPalService
    {
        private readonly PayPalSettings _settings;
        public PayPalService(IOptions<PayPalSettings> settings)
        {
            _settings = settings.Value;
        }
        private Dictionary<string, string> ObtenerConfiguracion()
        {
            return new Dictionary<string, string>()
        {
            { "mode", _settings.Mode },
            { "clientId", _settings.ClientId },
            { "clientSecret", _settings.ClientSecret }
        };
        }
        public async Task<string> ObtenerTokenAcceso()
        {
            var clientId = "ARmMGXgg8hcczfbIzc2sY0BOQcsNCx9BFlFYrN2gtczGS8DcZyl7y0oiRoJmFUB-s_BthWF9so1Uj3oB";
            var clientSecret = "EGKKWaC8lbZ-6Dxny3Vj0dK-1O9xQLx7zoWCBz9tk981VoNcNQflqEmoLJmnw2G3mui_AllP9ldlwOai";

            using var client = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var content = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("grant_type", "client_credentials")
    });

            // Usar sandbox o live según corresponda
            var response = await client.PostAsync("https://api-m.sandbox.paypal.com/v1/oauth2/token", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<PayPalTokenResponse>();
            return json.access_token;
        }
        private async Task<APIContext> ObtenerApiContext()
        {
            var accessToken = await ObtenerTokenAcceso();
            return new APIContext(accessToken)
            {
                Config = ObtenerConfiguracion()
            };
        }
        public async Task<Payment> CrearPago(decimal monto, string descripcion, string returnUrl, string cancelUrl)
        {
            var apiContext = await ObtenerApiContext();

            var payment = new Payment()
            {
                intent = "sale",
                payer = new Payer() { payment_method = "paypal" },
                transactions = new List<Transaction>()
            {
                new Transaction()
                {
                    description = descripcion,
                    invoice_number = System.Guid.NewGuid().ToString(),
                    amount = new Amount()
                    {
                        currency = "USD",
                        total = monto.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
                    }
                }
            },
                redirect_urls = new RedirectUrls()
                {
                    cancel_url = cancelUrl,
                    return_url = returnUrl
                }
            };

            var createdPayment = payment.Create(apiContext);
            return createdPayment;
        }
        public async Task<Payment> EjecutarPago(string paymentId, string payerId)
        {
            var apiContext = await ObtenerApiContext();
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            var payment = new Payment() { id = paymentId };
            var executedPayment = payment.Execute(apiContext, paymentExecution);
            return executedPayment;
        }
    }
}

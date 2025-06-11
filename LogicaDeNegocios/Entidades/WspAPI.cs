using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace LogicaDeNegocios.Entidades
{
    public class WspAPI
    {
        public void EnviarMensajeWhatsApp(string msg)
        {
 
            const string accountSid = "AC24965d244d19f2fa5af9141460b50520";
            const string authToken = "8a9eb5a0714900fee64d718beb6201b1";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                from: new PhoneNumber("whatsapp:+14155238886"), // Número sandbox Twilio
                to: new PhoneNumber("whatsapp:+59892274808"),   // Tu número personal
                body: msg
            );

            Console.WriteLine($"Mensaje enviado. SID: {message.Sid}");
        }
    }
}

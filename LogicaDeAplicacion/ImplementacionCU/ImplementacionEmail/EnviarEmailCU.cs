using LogicaDeAplicacion.InterfacesCU.InterfacesEmail;
using OpenQA.Selenium.DevTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeAplicacion.ImplementacionCU.ImplementacionEmail
{
    public class EnviarEmailCU : IEnviarEmail
    {
        public void Ejecutar(string destinatario, string asunto, string contenido)
        {
            using(SmtpClient cliente = new SmtpClient("smtp.gmail.com", 587)){
                cliente.Credentials = new NetworkCredential("", ""); // No funciona por no tener puesto email y contraseña por obvias razones
                cliente.EnableSsl = true;

                MailMessage mensaje = new MailMessage();
                mensaje.From = new MailAddress("");
                mensaje.To.Add(destinatario);
                mensaje.Subject = asunto;
                mensaje.Body = contenido;
                mensaje.IsBodyHtml = true;

                cliente.Send(mensaje);
            }
        }
    }
}

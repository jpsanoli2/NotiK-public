using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeAplicacion.InterfacesCU.InterfacesEmail
{
    public interface IEnviarEmail
    {
        public void Ejecutar(string destinatario, string asunto, string contenido);
    }
}

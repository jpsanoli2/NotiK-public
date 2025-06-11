using LogicaDeNegocios.Entidades;

namespace Noticias.ViewModels
{
    public class DivisaRiesgoViewModel
    {
        public IEnumerable<EnumDivisa> Divisas { get; set; }
        public IEnumerable<EnumRiesgo> Riesgos { get; set;}
    }
}

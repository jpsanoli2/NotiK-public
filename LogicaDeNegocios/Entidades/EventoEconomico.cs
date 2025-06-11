using LogicaDeNegocios.Entidades;
using LogicaDeNegocios.InterfacesEntidades;

public class EventoEconomico: IIdentify
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public EnumDivisa Divisa { get; set; }
    public EnumRiesgo Riesgo { get; set; }

    public EventoEconomico() { }
    public EventoEconomico(DateTime fecha, string nombre, string descripcion, EnumDivisa divisa, EnumRiesgo riesgo)
    {
        Fecha = fecha;
        Nombre = nombre;
        Descripcion = descripcion;
        Divisa = divisa;
        Riesgo = riesgo;
    }

}
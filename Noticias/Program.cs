using Microsoft.EntityFrameworkCore;
using AccesoADatos;
using LogicaDeNegocios.InterfacesRepositorios;
using LogicaDeAplicacion.InterfacesCU.InterfacesUsuario;
using LogicaDeAplicacion.ImplementacionCU.ImplementacionUsuario;
using LogicaDeAplicacion.ImplementacionCU.ImplementacionEventosEconomicos;
using LogicaDeAplicacion.InterfacesCU.InterfacesEventosEconomicos;
using LogicaDeAplicacion.InterfacesCU.InterfacesEmail;
using LogicaDeAplicacion.ImplementacionCU.ImplementacionEmail;
using LogicaDeNegocios.Entidades;
using PayPal.Api;
using LogicaDeNegocios.InterfacesEntidades;
using LogicaDeAplicacion.ImplementacionCU.ImplementacionMensajes;


namespace Noticias
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<DbContext, Contexto>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("StringConnection"))

            );
            builder.Services.Configure<TwilioOptions>(builder.Configuration.GetSection("Twilio"));
            builder.Services.AddHostedService<mensajeriawhatsappbackgroundservice>();


            builder.Services.Configure<PayPalSettings>(builder.Configuration.GetSection("PayPal"));
            builder.Services.AddSingleton<IPayPalService, PayPalService>();

            builder.Services.AddScoped(typeof(IRepositorioUsuario), typeof(RepositorioUsuario));
            builder.Services.AddScoped(typeof(IRepositorioEventoEconomico), typeof(RepositorioEventoEconomico));
            builder.Services.AddScoped(typeof(IRepositorioSuscripcion), typeof(RepositorioSuscripcion));

            builder.Services.AddScoped(typeof(IAgregarTelefono), typeof(AgregarTelefono));
            builder.Services.AddScoped(typeof(IAltaUsuario), typeof(AltaUsuario));
            builder.Services.AddScoped(typeof(ICambiarPass), typeof(CambiarPass));
            builder.Services.AddScoped(typeof(IEliminarUsuario), typeof(EliminarUsuario));
            builder.Services.AddScoped(typeof(IGetByEmail), typeof(GetByEmail));
            builder.Services.AddScoped(typeof(IGetDivisas), typeof(GetDivisas));
            builder.Services.AddScoped(typeof(IGetRiesgos), typeof(GetRiesgos));
            builder.Services.AddScoped(typeof(IGetUsuario), typeof(GetUsuario));
            builder.Services.AddScoped(typeof(ILogin), typeof(LoginCU));
            builder.Services.AddScoped(typeof(IModificarDivisas), typeof(ModificarDivisas));
            builder.Services.AddScoped(typeof(IModificarRiesgos), typeof(ModificarRiesgos));
            builder.Services.AddScoped(typeof(IModificarUsuario), typeof(ModificarUsuario));
            builder.Services.AddScoped(typeof(IOlvidePass), typeof(OlvidePass));
            builder.Services.AddScoped(typeof(IRecuperarPass), typeof(RecuperarPass));
            
            

            builder.Services.AddScoped(typeof(IGetEventoEconomico), typeof(GetEventoEconomico));
            builder.Services.AddScoped(typeof(IGetAllEventoEconomico), typeof(GetAllEventoEconomico));

            builder.Services.AddScoped(typeof(IEnviarEmail), typeof(EnviarEmailCU));
            builder.Services.AddScoped(typeof(IVerificarCuenta), typeof(VerificarUsuario));

            

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Host.UseWindowsService();


            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}

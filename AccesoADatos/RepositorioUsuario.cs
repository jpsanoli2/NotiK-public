using LogicaDeNegocios.Entidades;
using LogicaDeNegocios.Excepciones;
using LogicaDeNegocios.InterfacesRepositorios;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium.DevTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoADatos
{
    public class RepositorioUsuario : RepositorioGenerico<Usuario>, IRepositorioUsuario
    {
        private DbContext _context;
        public RepositorioUsuario(DbContext contexto) : base(contexto)
        {
            _context = contexto;
        }

        public async Task<Usuario> Modificar(int id, Usuario u)
        {
            Usuario? usuarioAModificar = await _context.Set<Usuario>()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (usuarioAModificar != null)
            {
                usuarioAModificar.Copiar(u);
                _context.Entry(usuarioAModificar).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return usuarioAModificar;
            }
            else
            {
                throw new NotFoundException("Not found.");
            }
        }

        public async Task<Usuario> Login(string email, string password)
        {
            Usuario? usuario = await _context.Set<Usuario>().FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(password, usuario.PassHash))
            {
                throw new NotValidException("User not found.");
            }
            else if(usuario.EmailVerificado == false)
            {
                throw new NotValidException("You must verify your account before logging in.");
            }

            return usuario;
        }
        public async Task<Usuario> Alta(Usuario usuario)
        {
            Usuario? user = await _context.Set<Usuario>().FirstOrDefaultAsync(u => u.Email == usuario.Email);
            if (user == null)
            {
                await _context.Set<Usuario>().AddAsync(usuario);
                await _context.SaveChangesAsync();
                return usuario;
            }
            return null;
        }
        public async Task<bool> VerificarCuenta(string token)
        {

            Usuario? usuario = await _context.Set<Usuario>().FirstOrDefaultAsync(u => u.TokenVerificacion == token);

            if (usuario == null || usuario.EmailVerificado)
            {
                return false;
            }

            usuario.EmailVerificado = true;
            usuario.TokenVerificacion = null;
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task AgregarTelefono(string telefono, int id)
        {
            Usuario? usuarioAModificar = await _context.Set<Usuario>()
                    .FirstOrDefaultAsync(t => t.Id == id);

            if (usuarioAModificar != null)
            {

                usuarioAModificar.Telefono = telefono;
                _context.Entry(usuarioAModificar).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("User not found.");
            }
           
        }
        public async Task CambiarPass(string pass, int id)
        {
            Usuario? usuarioAModificar = await _context.Set<Usuario>().FirstOrDefaultAsync(t => t.Id == id);
            if(usuarioAModificar != null)
            {
                usuarioAModificar.CambiarPass(pass);
                _context.Entry(usuarioAModificar).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("User not found.");
            }
        }
        public async Task RecuperarPass(Usuario usuario)
        {
            if (usuario != null)
            {
                string token = Guid.NewGuid().ToString();
                usuario.TokenRecuperacion = token;
                _context.Entry(usuario).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Usuario> GetByEmail(string email)
        {
            return await _context.Set<Usuario>().FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task OlvidePass(string token, string pass)
        {

            Usuario? usuario = await _context.Set<Usuario>().FirstOrDefaultAsync(u => u.TokenVerificacion == token);

            if (usuario == null)
            {
                throw new NotValidException("The user is not valid.");
            }
            usuario.CambiarPass(pass);
            usuario.TokenRecuperacion = null;
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task ModificarDivisas(List<EnumDivisa> divisas, int id)
        {
            Usuario? usuarioAModificar = await _context.Set<Usuario>()
                    .FirstOrDefaultAsync(t => t.Id == id);

            if (usuarioAModificar != null)
            {

                usuarioAModificar.DivisasNotificaciones = divisas;
                _context.Entry(usuarioAModificar).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("User not found.");
            }
        }
        public async Task ModificarRiesgos(List<EnumRiesgo> riesgos, int id)
        {
            Usuario? usuarioAModificar = await _context.Set<Usuario>()
                    .FirstOrDefaultAsync(t => t.Id == id);
            if (usuarioAModificar != null)
            {
                usuarioAModificar.RiesgoNotificaciones = riesgos;
                _context.Entry(usuarioAModificar ).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("User not found.");
            }
        }
        public async Task<IEnumerable<Usuario>> GetAllSuscriptores()
        {
            return await _context.Set<Usuario>()
                                 .Where(t => t.Suscrito == true)
                                 .ToListAsync();
        }


        public async Task<Usuario> GetBySubId(string subscriptionId)
        {
            return await _context.Set<Usuario>().FirstOrDefaultAsync(u => u.SubscriptionPayPalId == subscriptionId);
        }
        public async Task<IEnumerable<EnumDivisa>> GetDivisas(int id)
        {
            Usuario usuario = await Get(id);
            if(usuario != null)
            {
                return usuario.DivisasNotificaciones;
            }
            return new List<EnumDivisa>();
        }
        public async Task<IEnumerable<EnumRiesgo>> GetRiesgos(int id)
        {
            Usuario usuario = await Get(id);
            if(usuario != null)
            {
                return usuario.RiesgoNotificaciones;
            }
            return new List<EnumRiesgo>();
        }

        
    }
}

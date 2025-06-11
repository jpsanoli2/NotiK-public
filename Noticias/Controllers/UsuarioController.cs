using Dto;
using LogicaDeAplicacion.InterfacesCU.InterfacesUsuario;
using LogicaDeNegocios.Entidades;
using LogicaDeNegocios.Excepciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Identity.Client;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using LogicaDeNegocios.InterfacesRepositorios;
using Noticias.ViewModels;

namespace Noticias.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IAltaUsuario _altaUsuario;
        private readonly IGetUsuario _getUsuario;
        private readonly IGetByEmail _getByEmail;
        private readonly IEliminarUsuario _eliminarUsuario;
        private readonly IModificarUsuario _modificarUsuario;
        private readonly ILogin _login;
        private readonly IVerificarCuenta _verificarCuenta;
        private readonly IAgregarTelefono _agregarTelefono;
        private readonly ICambiarPass _cambiarPass;
        private readonly IRecuperarPass _recuperarPass;
        private readonly IOlvidePass _olvidePass;
        private readonly IModificarDivisas _modificarDivisas;
        private readonly IRepositorioSuscripcion _suscripcionService;
        private readonly IGetDivisas _getDivisas;
        private readonly IGetRiesgos _getRiesgos;
        private readonly IModificarRiesgos _modificarRiesgos;

        public UsuarioController(IAltaUsuario altaUsuario, IGetUsuario getUsuario, IEliminarUsuario eliminarUsuario, IModificarUsuario modificarUsuario,
                                 ILogin login, IVerificarCuenta verificarCuenta, IAgregarTelefono agregarTelefono, ICambiarPass cambiarPass, IRecuperarPass recuperarPass,
                                 IGetByEmail getByEmail, IOlvidePass olvidePass, IModificarDivisas modificarDivisas, IRepositorioSuscripcion suscripcionService,
                                 IGetDivisas getDivisas, IGetRiesgos getRiesgos, IModificarRiesgos modificarRiesgos)
        {
            _altaUsuario = altaUsuario;
            _getUsuario = getUsuario;
            _eliminarUsuario = eliminarUsuario;
            _modificarUsuario = modificarUsuario;
            _login = login;
            _verificarCuenta = verificarCuenta;
            _agregarTelefono = agregarTelefono;
            _cambiarPass = cambiarPass;
            _recuperarPass = recuperarPass;
            _getByEmail = getByEmail;
            _olvidePass = olvidePass;
            _modificarDivisas = modificarDivisas;
            _suscripcionService = suscripcionService;
            _getDivisas = getDivisas;
            _getRiesgos = getRiesgos;
            _modificarRiesgos = modificarRiesgos;
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            UsuarioDto user;
            try
            {
                user = await _login.Ejecutar(Email, Password);

            }
            catch (NotValidException ex)
            {
                ViewBag.msg = ex.Message;
                return View();
            }

            HttpContext.Session.SetInt32("UsuarioId", user.Id);
            HttpContext.Session.SetString("Nombre", user.Nombre);
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("Telefono", user.Telefono);
            HttpContext.Session.SetString("Suscrito", user.Suscrito.ToString());
            HttpContext.Session.SetString("FechaSuscripcion", user.FechaSuscripcion.ToString("o")); 

            return RedirectToAction("Inicio", "Usuario");

        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View(new UsuarioAltaDto());
        }

        [HttpPost]
        public async Task<IActionResult> Registro(UsuarioAltaDto user, string ConfirmPassword)
        {
            if (user.Password != ConfirmPassword)
            {
                TempData["errorRegistro"] = "The passwords do not match.";
                return View(user);
            }
            try
            {
                await _altaUsuario.Ejecutar(user);
                TempData["registroExitoso"] = "Verify your email to complete the registration.";
                return RedirectToAction("Login", "Usuario");
            }
            catch (ArgumentException ex)
            {
                TempData["errorRegistro"] = ex.Message;
                return View(user);
            }
            catch(NotValidException notValidEx)
            {
                TempData["errorRegistro"] = notValidEx.Message;
                return View(user);
            }
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Usuario");
        }
        [HttpGet]
        public async Task<IActionResult> VerificarCuenta(string token)
        {
            try
            {
                bool verificado = await _verificarCuenta.Ejecutar(token);
                if (verificado)
                {
                    ViewBag.Mensaje = "Your account has been successfully verified.";
                }
                else
                {
                    ViewBag.Mensaje = "The token is invalid or has already been used.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = "An error occurred while verifying the account:" + ex.Message;
            }

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Inicio()
        {
            int id = HttpContext.Session.GetInt32("UsuarioId") ?? 0;
            try
            {
                UsuarioDto usuario = await _getUsuario.Ejecutar(id);
                return View(usuario);
            }
            catch(Exception e)
            {
                return View();
            }
            
            
            
        }
        [HttpGet]
        public async Task<IActionResult> Perfil()
        {
            int id = HttpContext.Session.GetInt32("UsuarioId") ?? 0;
            try
            {
                UsuarioDto uDto = await _getUsuario.Ejecutar(id);
                return View(uDto);
            }
            catch(Exception e)
            {
                return View();
            }
           
        }
        [HttpPost]
        public async Task<IActionResult> Perfil(string telefono, string newPass, string confirmPass)
        {
            int? sessionId = HttpContext.Session.GetInt32("UsuarioId");
            if (sessionId == null || sessionId == 0)
            {
                return RedirectToAction("Login", "Usuario");
            }
            int id = sessionId.Value;

            UsuarioDto usuario = await _getUsuario.Ejecutar(id);

            if (usuario == null)
            {
                ViewBag.msg = "User not found.";
                return View(usuario);
            }
            if (!string.IsNullOrEmpty(telefono) && usuario.Telefono != telefono)
            {
                await _agregarTelefono.Ejecutar(telefono, id);
                ViewBag.msg = "Phone added successfully.";
            }
            if (!string.IsNullOrEmpty(newPass) || !string.IsNullOrEmpty(confirmPass))
            {
                if (string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
                {
                    ViewBag.msg = "You must complete both password fields.";
                    return View(usuario);
                }
                if (newPass.Length < 6)
                {
                    ViewBag.msg = "The new password must contain at least 6 characters.";
                    return View(usuario);
                }
                if (newPass != confirmPass)
                {
                    ViewBag.msg = "The passwords do not match.";
                    return View(usuario);
                }
                _cambiarPass.Ejecutar(newPass, id);
                ViewBag.msg = "Changes saved successfully.";
            }
            return View(usuario);
        }
        [HttpGet]
        public IActionResult OlvidePass()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> OlvidePass(string email)
        {
            UsuarioDto usuarioDto = await _getByEmail.Ejecutar(email);
            if (usuarioDto != null)
            {
                try
                {
                    _recuperarPass.Ejecutar(usuarioDto);
                    ViewBag.msg = "Verify your email to recover your password.";
                    return View();
                }
                catch (Exception ex)
                {
                    TempData["errorRegistro"] = ex.Message;
                    return View();
                }
            }
            else
            {
                ViewBag.msgError = "The email is not registered.";
                return View();
            }
        }
        [HttpGet]
        public IActionResult RecuperarPass()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RecuperarPass(string token, string pass, string confirmPass)
        {

            if (pass == confirmPass)
            {
                try
                {
                    _olvidePass.Ejecutar(token, pass);
                    TempData["registroExitoso"] = "Password changed successfully.";
                    return RedirectToAction("Login", "Usuario");
                }
                catch (Exception ex)
                {
                    ViewBag.msg = ex.Message;
                }
            }
            else if (pass != confirmPass)
            {
                ViewBag.msg = "The passwords do not match.";
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ConfigurarBot()
        {

            int id = HttpContext.Session.GetInt32("UsuarioId") ?? 0;
            var model = new DivisaRiesgoViewModel
            {
                Divisas = await _getDivisas.Ejecutar(id),
                Riesgos = await _getRiesgos.Ejecutar(id)
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ConfigurarBot(List<EnumDivisa> DivisasSeleccionadas, List<EnumRiesgo> RiesgosSeleccionados)
        {
            int id = HttpContext.Session.GetInt32("UsuarioId") ?? 0;
            try
            {
                await _modificarDivisas.Ejecutar(DivisasSeleccionadas, id);
                await _modificarRiesgos.Ejecutar(RiesgosSeleccionados, id);
                
                ViewBag.msg = "Data saved successfully.";
            }
            catch (NotFoundException ex)
            {
                ViewBag.msg = ex.Message;
            }
            var model = new DivisaRiesgoViewModel
            {
                Divisas = DivisasSeleccionadas,
                Riesgos = RiesgosSeleccionados
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ActivarSuscripcion([FromBody] ActivarSuscripcionDto dto)
        {
            int usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;

            if (usuarioId == 0 || string.IsNullOrEmpty(dto.SubscriptionId))
                return BadRequest("Invalid data.");

            await _suscripcionService.GuardadSubscriptionId(usuarioId, dto.SubscriptionId);

            return Ok();
        }
    }

}

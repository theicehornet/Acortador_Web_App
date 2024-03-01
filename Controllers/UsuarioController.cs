using Acortador_Web_App.Models;
using Acortador_Web_App.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Acortador_Web_App.Controllers
{
    public class UsuarioController(IUsuarioService usuarioService, IAcortadorService acortadorService) : Controller
    {
        private readonly IAcortadorService _acortadorService = acortadorService;
        public IActionResult Registrarse() => View();

        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario user)
        {
            user.Password = Utils.Utilities.EncodePassword(user.Password);
            user.Id = Guid.NewGuid().ToString();
            try
            {
                Usuario usercreated = await usuarioService.SaveUsuario(user);
                HttpContext.Session.SetString("idUser", user.Id);
                HttpContext.Session.SetString("isAdmin", user.IsAdministrador.ToString());
                return RedirectToAction("Index", "Acortador");
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message.ToString();
                return View();
            }
            
        }

        public IActionResult IniciarSesion() => View();

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string email,string password)
        {
            try
            {
                password = Utils.Utilities.EncodePassword(password);
                Usuario user = await usuarioService.GetUsuario(email, password);
                if (user == null)
                {
                    ViewData["MessageLogin"] = "El email o la contraseña no son correctos";
                }
                else
                {
                    ViewData["MessageLogin"] = "Se ha iniciado sesion correctamente";
                    HttpContext.Session.SetString("idUser", user.Id);
                    HttpContext.Session.SetString("isAdmin", user.IsAdministrador.ToString());
                }
                Console.WriteLine(Json(user).ToString());
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewData["Error"] = ex.Message;
            }
            return View();
        }
        public async Task<IActionResult> Perfil()
        {
            string? idUser = HttpContext.Session.GetString("idUser");
            if (idUser == null)
            {
                TempData["Error"] = "Inicie session porfavor";
                return RedirectToAction("IniciarSesion");
            }
            Usuario user = await usuarioService.GetUsuario(idUser);
            List<Acortador> acortadores;
            if (!user.IsAdministrador)
            {
                ViewData["Title"] = "Perfil";
                acortadores = await _acortadorService.ObtenerAcortadoresUsuario(idUser);
                return View(acortadores);
            }
            acortadores = await _acortadorService.ObtenerAcortadores();
            ViewData["Title"] = "Panel del Administrador";
            return View(acortadores);
        }
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("IniciarSesion"); ;
        }

    }
}

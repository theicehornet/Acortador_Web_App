using Acortador_Web_App.Models;
using Acortador_Web_App.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Acortador_Web_App.Controllers
{
    public class UsuarioController(ShorturlContext context, IEmailService emailService,IUsuarioService usuarioService) : Controller
    {
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
                Console.WriteLine(ex.ToString());
            }
            return View();
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("IniciarSesion"); ;
        }


        public async Task<IActionResult> PanelAcortadores()
        {
            string? idUser = HttpContext.Session.GetString("idUser");
            if (idUser == null)
            {
                TempData["Error"] = "Inicie session porfavor";
                return RedirectToAction("IniciarSesion");
            }else if(!context.Usuarios.Find(idUser).IsAdministrador)
            {
                TempData["Error"] = "Usted no es un administrador";
                return RedirectToAction("IniciarSesion");
            }
            List<Acortador> acortadores = await context.Acortadors.ToListAsync();
            return View(acortadores);
        }

        public async Task<IActionResult> EliminarEnlace(string id)
        {
            string? idUser = HttpContext.Session.GetString("idUser");
            if (idUser == null)
            {
                TempData["Error"] = "Inicie session porfavor";
                return RedirectToAction("IniciarSesion");
            }
            else if (context.Usuarios.Find(idUser)?.IsAdministrador == null)
            {
                TempData["Error"] = "Usted no es un administrador";
                return RedirectToAction("IniciarSesion");
            }
            var ac = context.Acortadors.FindAsync(id);
            var obj = await ac;
            context.Acortadors.Remove(obj);
            context.SaveChanges();
            return Redirect("/Administrador/Index");
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
            return View(user);
        }
    }
}

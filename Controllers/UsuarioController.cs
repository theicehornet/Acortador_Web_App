using Acortador_Web_App.Models;
using Acortador_Web_App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Acortador_Web_App.Controllers
{
    public class UsuarioController(ShorturlContext context, IEmailService emailService) : Controller
    {
        public IActionResult Registrarse() => View();

        [HttpPost]
        public IActionResult Registrarse(Usuario user)
        {
            HttpContext.Session.SetString("idUser", user.Id);
            return View();
        }

        public IActionResult IniciarSesion() => View();

        [HttpPost]
        public IActionResult IniciarSesion(string email,string password)
        {
            try
            {
                Usuario? user = context.Usuarios.FirstOrDefault(user => user.Email == email);
                if (user == null)
                {
                    ViewData["MessageLogin"] = "Usted no se encuentra registrado";
                }
                else if (user.Password != password)
                {
                    ViewData["MessageLogin"] = "Ingrese correctamente su contraseña";
                }
                else
                {
                    ViewData["MessageLogin"] = "Se ha iniciado sesion correctamente";
                    HttpContext.Session.SetString("idUser", user.Id);
                }
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
            ViewData["Title"] = "Panel del Administrador";
            List<Acortador> acortadores = await context.Acortadors.ToListAsync();
            return View(acortadores);
        }

        public IActionResult EliminarEnlace(string id)
        {
            string? idUser = HttpContext.Session.GetString("idUser");
            if (idUser == null)
            {
                TempData["Error"] = "Inicie session porfavor";
                return RedirectToAction("IniciarSesion");
            }
            else if (context.Acortadors.Find(idUser)?.Lasttime == null)
            {
                TempData["Error"] = "Usted no es un administrador";
                return RedirectToAction("IniciarSesion");
            }
            var ac = context.Acortadors.Find(id);
            context.Acortadors.Remove(ac);
            context.SaveChanges();
            return Redirect("/Administrador/Index");
        }




    }
}

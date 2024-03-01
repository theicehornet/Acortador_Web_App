using Acortador_Web_App.Models;
using Acortador_Web_App.Services.Contract;
using Acortador_Web_App.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static QRCoder.PayloadGenerator;

namespace Acortador_Web_App.Controllers;

public class AcortadorController(IUsuarioService usuarioService, IEmailService emailService, IAcortadorService acortadorService) : Controller
{

    private readonly IEmailService _emailService = emailService;

    private readonly IAcortadorService _acortadorService = acortadorService;

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("/{id}")]
    public async Task<IActionResult> Index(string id)
    {
        if (id == null || id == "Acortador_Web_App.styles.css") return View();
        try
        {
            Acortador url = await _acortadorService.UpdateAcortador(id);
            _emailService.SendEmail(new EmailDTO("algun correo", "Nuevo uso del acortador", $"Se ha usado el acortador con id: {id}, el link es {url.Link}"));
            if (Utilities.IsURL(url.Link))
                return Redirect(url.Link);
            else
                return Json(url.Link);
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> CrearAcortador(string link)
    {
        try
        {
            string? idUser = HttpContext.Session.GetString("idUser");
            Acortador acor = await _acortadorService.CreateAcortador(link, idUser);
            if (idUser == null)
            {
                _emailService.SendEmail(new EmailDTO("algun correo", "Creado Acortador", $"Se ha creado un nuevo acortador este es el enlace 'https://AcotadorURL.somee.com/{acor.Id}'"));
            }
            else
            {
                Usuario user = await usuarioService.GetUsuario(idUser);
                _emailService.SendEmail(new EmailDTO(user.Email, "Creado Acortador", $"Se ha creado un nuevo acortador este es el enlace 'https://AcotadorURL.somee.com/{acor.Id}'"));
            }
            ViewBag.Qr = Utilities.CrearQr("https://AcotadorURL.somee.com/" + acor.Id);
            return View("Index", acor);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            ViewBag.Error = ex.Message;
            return View("Index");
        }
    }

    public async Task<IActionResult> EliminarEnlace(string id)
    {
        string? idUser = HttpContext.Session.GetString("idUser");
        if (idUser == null)
        {
            TempData["Error"] = "Inicie session porfavor";
            return RedirectToAction("IniciarSesion", "Usuario");
        }
        try
        {
            Usuario user = await usuarioService.GetUsuario(idUser);
            Acortador ac = await _acortadorService.GetAcortador(id);
            if ((user.Id != ac.UserId) && !user.IsAdministrador)
                throw new Exception("Usted no es dueño del acortador o no es un administrador");
            _emailService.SendEmail(new EmailDTO(user.Email, "Acortador Eliminado", $"Se ha eliminado el acortador con enlace 'https://AcotadorURL.somee.com/{ac.Id}'"));
            await _acortadorService.EliminarAcortador(ac);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            TempData["Error"] = ex.Message;
            return RedirectToAction("Perfil", "Usuario");
        }
        return Redirect("/Usuario/Perfil");
    }

}

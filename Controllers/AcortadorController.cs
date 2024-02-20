using Acortador_Web_App.Models;
using Acortador_Web_App.Services.Contract;
using Acortador_Web_App.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Acortador_Web_App.Controllers;

public class AcortadorController : Controller
{
    private readonly ShorturlContext _context;

    private readonly IEmailService _emailService;

    public AcortadorController(ShorturlContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("/{id}")]
    public async Task<IActionResult> IndexAsync(string id)
    {
        if (id == null || id == "Acortador_Web_App.styles.css") return View();
        try
        {
            Acortador url = await _context.Acortadors.FindAsync(id);
            url.Lasttime = DateTime.Now;
            var saving = _context.SaveChangesAsync();
            _emailService.SendEmail(new EmailDTO("algun correo", "Nuevo uso del acortador", $"Se ha usado el acortador con id: {id}, el link es {url.Link}"));
            await saving;
            if (Utilities.IsURL(url.Link))
                return Redirect(url.Link);
            else
                return Json(url.Link);
        }
        catch
        {
            ViewBag.Error = "URL no encontrada";
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> CrearAcortador(string link)
    {
        Acortador acor = null;
        try
        {
            List<Acortador> acortadores = await _context.Acortadors.ToListAsync();
            if (Utilities.LinkExiste(link, acortadores))
            {
                acor = Utilities.HallarAcortador(link, acortadores);
            }
        }
        catch
        {
            acor = Utilities.CrearAcortador(link);
            _context.Add(acor);
            await _context.SaveChangesAsync();
        }
        ViewBag.Qr = Utilities.CrearQr("https://AcotadorURL.somee.com/" + acor.Id);
        return View("Index", acor);
    }
}

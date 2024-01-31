using Acortador_Web_App.Models;
using Acortador_Web_App.Services;
using Acortador_Web_App.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Acortador_Web_App.Controllers;

public class AcortadorController : Controller
{
    private readonly AcortadorurlContext _context;

    private readonly IEmailService _emailService;

    public AcortadorController(AcortadorurlContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("/{id}")]
    public async Task<IActionResult> Index(string id)
    {
        if (id == null) return View();
        try
        {
            Acortador url = await _context.Acortadors.FindAsync(id);
            url.Lasttime = DateTime.Now;
            var saving = _context.SaveChangesAsync();
            _emailService.SendEmail(new EmailDTO("ingrese un email", "Nuevo uso del acortador", "Se ha usado el acortador con id " + id));
            await saving;
            return Redirect(url.Link);
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
        ViewBag.Qr = Utilities.CrearQr("https://localhost:7234" + acor.Id);
        return View("Index", acor);
    }
}

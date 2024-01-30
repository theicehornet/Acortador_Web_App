using Acortador_Web_App.Models;
using Acortador_Web_App.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Acortador_Web_App.Controllers
{
    public class AcortadorController : Controller
    {
        private readonly AcortadorurlContext _context;

        public AcortadorController(AcortadorurlContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            Acortador? url = await _context.Acortadors.FindAsync(id);

            if (url == null)
            {
                ViewBag.Error = "URL no encontrada";
                return View();
            }
            return Redirect(url.Link);
        }

        [HttpPost]
        public async Task<IActionResult> CrearAcortadorAsync(string link)
        {
            Acortador? acor = null;
            try
            {
                List<Acortador> acortadores = await _context.Acortadors.ToListAsync();
                if (Utilities.LinkExiste(link, acortadores))
                {
                    acor = Utilities.HallarAcortador(link,acortadores);
                }
            }catch
            {
                acor = Utilities.CrearAcortador(link);
                _context.Add(acor);
                await _context.SaveChangesAsync();
            }
            ViewBag.Qr = Utilities.CrearQr(link);
            return View("Index", acor);
        }
    }
}

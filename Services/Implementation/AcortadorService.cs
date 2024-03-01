using Acortador_Web_App.Models;
using Acortador_Web_App.Services.Contract;
using Acortador_Web_App.Utils;
using Microsoft.EntityFrameworkCore;

namespace Acortador_Web_App.Services.Implementation
{
    public class AcortadorService(ShorturlContext dbContext) : IAcortadorService
    {
        private readonly ShorturlContext _dbContext = dbContext;

        public async Task<Acortador> CreateAcortador(string link,string? usuarioId)
        {
            Acortador ac = Utilities.CrearAcortador(link);
            if (usuarioId == null)
                ac.UserId = null;
            else
                ac.UserId = usuarioId;
            await _dbContext.AddAsync(ac);
            await _dbContext.SaveChangesAsync();
            return ac;
        }

        public async Task<Acortador> GetAcortador(string id)
        {
            Acortador? ac = await _dbContext.Acortadors.FindAsync(id);
            return ac ?? throw new Exception($"El acortador con el ID: {id} no existe.");
        }

        public async Task<List<Acortador>> ObtenerAcortadores()
        {
            List<Acortador> acortadores = await _dbContext.Acortadors.ToListAsync();
            return acortadores;
        }

        public async Task<List<Acortador>> ObtenerAcortadoresUsuario(string idUser)
        {
            List<Acortador> acortadores = await ObtenerAcortadores();
            List<Acortador> acortadoresUsuario = (List<Acortador>)acortadores.Where((Acortador acortador) => acortador.UserId == idUser);
            return acortadoresUsuario;
        }


        public async Task<string> EliminarAcortador(string id)
        {
            try
            {
                Acortador ac = await GetAcortador(id);
                _dbContext.Acortadors.Remove(ac);
                await _dbContext.SaveChangesAsync();
                return "Se elimino el acortador exitosamente.";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<string> EliminarAcortador(Acortador acortador)
        {
            try
            {
                _dbContext.Acortadors.Remove(acortador);
                await _dbContext.SaveChangesAsync();
                return "Se elimino el acortador exitosamente.";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Acortador> UpdateAcortador(string id)
        {
            Acortador ac = await GetAcortador(id);
            ac.Lasttime = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return ac;
        }

       
    }
}

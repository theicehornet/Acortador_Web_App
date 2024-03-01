using Acortador_Web_App.Models;

namespace Acortador_Web_App.Services.Contract
{
    public interface IAcortadorService
    {
        Task<Acortador> CreateAcortador(string link,string? usuarioId);
        Task<List<Acortador>> ObtenerAcortadores();
        Task<List<Acortador>> ObtenerAcortadoresUsuario(string idUser);
        Task<Acortador> GetAcortador(string id);
        Task<string> EliminarAcortador(string id);
        Task<string> EliminarAcortador(Acortador acortador);
        Task<Acortador> UpdateAcortador(string id);
    }
}

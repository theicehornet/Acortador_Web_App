using Microsoft.EntityFrameworkCore;
using Acortador_Web_App.Models; 

namespace Acortador_Web_App.Services.Contract
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuario(string email,string password);

        Task<Usuario> GetUsuario(string id);

        Task<Usuario> SaveUsuario(Usuario user);
    }
}

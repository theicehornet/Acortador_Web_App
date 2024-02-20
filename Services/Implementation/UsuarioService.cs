﻿using Microsoft.EntityFrameworkCore;
using Acortador_Web_App.Models;
using Acortador_Web_App.Services.Contract;

namespace Acortador_Web_App.Services.Implementation
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ShorturlContext _dbContext;

        public UsuarioService(ShorturlContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Usuario> GetUsuario(string email, string password)
        {
            Usuario user_found = await _dbContext.Usuarios.Where(u => u.Email == email && u.Password == password)
                .FirstOrDefaultAsync();
            return user_found;
        }

        public async Task<Usuario> GetUsuario(string id)
        {
            Usuario user_found = await _dbContext.Usuarios.Where(u => u.Id == id)
                .FirstOrDefaultAsync();
            return user_found;
        }

        public async Task<Usuario> SaveUsuario(Usuario user)
        {
            _dbContext.Usuarios.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}

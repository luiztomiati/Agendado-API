using Agendado.Data;
using Agendado.Dto;
using Agendado.Interface.Repository;
using Agendado.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Agendado.Repository
{
    
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CriarUsuario(Usuario dados)
        {
            _context.Add(dados);
        }

        public async Task<Usuario?> GetUsuarioByIdAsync(Guid id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(f => f.Id == id);
            return usuario;
        }

        public async Task<Usuario> UpdateUsuarioAsync(Usuario usuario)
        {
            _context.Update(usuario);
            _context.SaveChanges();
            return usuario;
        }
        public async Task SalvarUsuarioAsync(Usuario dados)
        {
            _context.Add(dados);
            _context.SaveChanges();
        }
        public async Task DeleteUsuarioAsync(Usuario usuario)
        {
            _context.Remove(usuario);
            _context.SaveChanges();
        }

        public async Task<Usuario?> GetIdentityUserAsync(string id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(f => f.IdentityUserId == id);
            return usuario;
        }
    }
}

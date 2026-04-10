using Agendado.Data;
using Agendado.Dto;
using Agendado.Interface;
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
            this._context = context;
        }

        public void CriarUsuario(Usuario dados)
        {
            try
            {
                _context.Add(dados);
            }
            catch (Exception ex) { 
                throw new Exception(ex.Message);
            }
        }

        public Usuario GetUsuario(Guid id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(f => f.Id == id)?? throw new Exception("Usuário não encontrado");
            return usuario;
        }

        public Usuario PutUsuario(Guid id, DadosEditUsuario dados, Usuario usuario)
        {
            usuario.Nome = dados.Nome;
            usuario.Email = dados.Email;
            usuario.Telefone = dados.Telefone;
            usuario.DDD = dados.DDD;
            _context.SaveChanges();
            return usuario;
        }
        public void SalvarUsuario(Usuario dados)
        {
            try
            {
                _context.Add(dados);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void DelUsuario(Guid id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(f => f.Id == id) ?? throw new Exception("Usuário não encontrado");
            _context.Remove(usuario);
            _context.SaveChanges();
        }

        public Usuario GetIdentityUser(string id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(f => f.IdentityUserId == id)
                ?? throw new Exception("Usuário não foi encontrado");
            return usuario;
        }
    }
}

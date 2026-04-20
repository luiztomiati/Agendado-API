using Agendado.Application.Dto;
using Agendado.Data;
using Agendado.Domain.Model;
using Agendado.Interface.Repository;
using Agendado.Shared;
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
            return _context.Usuarios.FirstOrDefault(f => f.Id == id);
        }

        public async Task<Usuario> UpdateUsuarioAsync(Usuario usuario)
        {
            _context.Update(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
        public async Task SalvarUsuarioAsync(Usuario dados)
        {
            _context.Add(dados);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUsuarioAsync(Usuario usuario)
        {
            _context.Remove(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario?> GetIdentityUserAsync(string id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(f => f.IdentityUserId == id);
            return usuario;
        }

        public async Task<ResultadoPagincao<DadosUsuarioResponse>> GetUsuariosAsync(Guid empresaId, int page, int qtdPage)
        {
            var query = _context.Usuarios.Where(w => w.EmpresaId == empresaId);

            var total = await query.CountAsync();

            var usuarios = await query
                .Skip((page - 1) * qtdPage)
                .Take(qtdPage)
                .Select(s => new DadosUsuarioResponse (
                    s.Id,
                    s.Nome,
                    s.Email,
                    s.DDD,
                    s.Telefone
                ))
                .ToListAsync();

            return new ResultadoPagincao<DadosUsuarioResponse> {
                Items = usuarios,
                TotalCount = total,
                Page = page,
                QtdPage = qtdPage
            };
        }
    }
}
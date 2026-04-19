using Agendado.Data;
using Agendado.Domain.Model;
using Agendado.Interface.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Agendado.Repository
{
    public class ServicoUsuarioRepository : IServicoUsuarioRepository
    {
        private readonly AppDbContext _context;

        public ServicoUsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task VincularServicoUsuarioAsync(ServicoUsuario servicoUsuario)
        {
            try
            {
                _context.Add(servicoUsuario);
            
                await _context.SaveChangesAsync();

            }catch(Exception e)
            {
                Console.WriteLine($"ERRO NO BANCO: {e.InnerException?.Message ?? e.Message}");
                throw;
            }
        }

        public async Task<bool> ExisteServicoUsuarioAsync(Guid usuarioId, Guid servicoId)
        {
            if(!_context.ServicoUsuarios.Any(f => f.UsuarioId ==  usuarioId && f.ServicoId == servicoId))
            {
                return true;
            }
            return false;
        }
    }
}

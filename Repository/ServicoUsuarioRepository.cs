using Agendado.Data;
using Agendado.Domain.Model;
using Agendado.Interface.Repository;

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
            _context.Add(servicoUsuario);
            await _context.SaveChangesAsync();
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

using Agendado.Domain.Model;

namespace Agendado.Interface.Repository
{
    public interface IServicoUsuarioRepository
    {
        Task VincularServicoUsuarioAsync(ServicoUsuario servicoUsuario);
        Task<bool> ExisteServicoUsuarioAsync(Guid usuarioId, Guid servicoId);
    }
}

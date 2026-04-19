using Agendado.Interface.Repository;

namespace Agendado.Interface.Service
{
    public interface IServicoUsuarioService
    {
        Task ValidarVinculoUsuarioServicoAsync(Guid usuarioId, Guid servicoId);
    }
}

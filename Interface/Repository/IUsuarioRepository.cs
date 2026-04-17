using Agendado.Domain.Model;

namespace Agendado.Interface.Repository
{
    public interface IUsuarioRepository
    {
        Task SalvarUsuarioAsync(Usuario dados);
        Task DeleteUsuarioAsync(Usuario usuario);
        Task<Usuario> UpdateUsuarioAsync(Usuario usuario);
        Task<Usuario?> GetUsuarioByIdAsync(Guid id);
        Task<Usuario?> GetIdentityUserAsync(string id);
        void CriarUsuario (Usuario usuario);

    }
}

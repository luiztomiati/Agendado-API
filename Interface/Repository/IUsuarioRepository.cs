using Agendado.Application.Dto;
using Agendado.Domain.Model;
using Agendado.Shared;


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
        Task<ResultadoPagincao<DadosUsuarioResponse>> GetUsuariosAsync(Guid empresaId, int page, int qtdPage);
    }
}

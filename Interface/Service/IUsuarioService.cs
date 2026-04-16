using Agendado.Dto;

namespace Agendado.Interface.Service
{
    public interface IUsuarioService
    {
        Task<DadosUsuarioResponse> CriarUsuarioAsync(DadosUsuarioRequest dados);
        Task DeletarUsuarioAsync(Guid usuarioId);
        Task<DadosEditUsuario> EditarUsuarioAsync(Guid usuarioId, DadosEditUsuario dados);
        Task ResetarPasswordAsync(DadosResetarSenhaRequest dados);
    }
}

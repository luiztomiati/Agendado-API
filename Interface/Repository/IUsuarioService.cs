using Agendado.Dto;

namespace Agendado.Interface.Repository
{
    public interface IUsuarioService
    {
        DadosUsuarioResponse CriarUsuario(DadosUsuarioRequest dados);
        void DeleteUsuario(Guid usuarioId);
        DadosEditUsuario EditUsuario(Guid usuarioId, DadosEditUsuario dados);
    }
}

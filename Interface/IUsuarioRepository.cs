using Agendado.Dto;
using Agendado.Model;

namespace Agendado.Interface
{
    public interface IUsuarioRepository
    {
        void CriarUsuario(Usuario dados);
        void SalvarUsuario(Usuario dados);
        void DelUsuario(Guid id);
        Usuario PutUsuario(Guid usuarioId, DadosEditUsuario dados, Usuario usuario);
        Usuario GetUsuario(Guid id);
        Usuario GetIdentityUser(string id);
        
    }
}

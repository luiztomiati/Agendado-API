using Agendado.Dto;

namespace Agendado.Interface.Repository
{
    public interface IEmpresaService
    {
        Task CriarEmpresaUsuarioAdmin(DadosEmpresaUsuarioAdmin dados);
    }
}

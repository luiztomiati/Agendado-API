using Agendado.Dto;

namespace Agendado.Interface.Service
{
    public interface IEmpresaService
    {
        Task CriarEmpresaUsuarioAdminAsync(DadosEmpresaUsuarioAdmin dados);
    }
}

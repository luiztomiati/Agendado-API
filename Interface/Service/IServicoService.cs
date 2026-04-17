using Agendado.Application.Dto;

namespace Agendado.Interface.Service
{
    public interface IServicoService
    {
        Task CriarServicoAsync(DadosServico dados);
        Task DeletarServicoAsync(Guid id);
        Task EditarServicoAsync(Guid id, DadosServico dados);
    }
}

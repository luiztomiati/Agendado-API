using Agendado.Application.Dto;
using Agendado.Domain.Model;
using Agendado.Shared;

namespace Agendado.Interface.Service
{
    public interface IServicoService
    {
        Task<Servicos> CriarServicoAsync(DadosServico dados);
        Task DeletarServicoAsync(Guid id);
        Task EditarServicoAsync(Guid id, DadosServico dados);
        Task<DadosServico> GetServicoByIdAsync(Guid id);
        Task<ResultadoPagincao<DadosServico>> ListServicosAsync(int page, int qtdPag);
    }
}

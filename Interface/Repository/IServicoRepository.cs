using Agendado.Application.Dto;
using Agendado.Domain.Model;
using Agendado.Shared;

namespace Agendado.Interface.Repository
{
    public interface IServicoRepository
    {
        Task SalvarServicoAsync(Servicos servico);
        Task<Servicos?> GetServicoByIdAsync(Guid id);
        Task UpdateServicoAsync(Servicos servico);
        Task DeleteServicoAsync(Servicos servico);
        Task<ResultadoPagincao<DadosServico>> GetServicosAsync(Guid empresaId, int page, int qtdPag);
    }
}

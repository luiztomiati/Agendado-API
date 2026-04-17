using Agendado.Domain.Model;

namespace Agendado.Interface.Repository
{
    public interface IServicoRepository
    {
        Task SalvarServicoAsync(Servicos servico);
        Task<Servicos?> GetServicoByIdAsync(Guid id);
        Task UpdateServicoAsync(Servicos servico);
        Task DeleteServicoAsync(Servicos servico);
    }
}

using Agendado.Domain.Model;

namespace Agendado.Interface.Repository
{
    public interface IAtendimentoRepository
    {
        void Save();
        List<Atendimento> GetListById(List<Guid> ids);
    }
}

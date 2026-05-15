using Agendado.Domain.Entities;

namespace Agendado.Interface.Repository
{
    public interface IEmpresaAtendimentoRepository
    {
        List<EmpresaFuncionamento> GetByEmpresaId(Guid empresaId);
        List<EmpresaFuncionamento> GetById(List<Guid> id);
    }
}

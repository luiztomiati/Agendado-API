using Agendado.Domain.Model;

namespace Agendado.Interface.Repository
{
    public interface IEmpresaRepository
    {
        void CriarEmpresa(Empresa empresa);
        Task<Empresa?> GetEmpresaByIdAsync(Guid id);
    }
}

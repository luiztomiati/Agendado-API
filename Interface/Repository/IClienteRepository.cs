using Agendado.Application.DTOs;
using Agendado.Domain.Model;
using Agendado.Shared;

namespace Agendado.Interface.Repository
{
    public interface IClienteRepository
    {
        Task<ResultadoPagincao<DadosCliente>> ListClientesAsync(Guid empresaId , int page, int qtdPag);
    }
}

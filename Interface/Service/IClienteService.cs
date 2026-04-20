using Agendado.Application.DTOs;
using Agendado.Domain.Model;
using Agendado.Shared;

namespace Agendado.Interface.Service
{
    public interface IClienteService
    {
        Task<ResultadoPagincao<DadosCliente>> GetClientesAsync(int page, int qtdPag);
    }
}

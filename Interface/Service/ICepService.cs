using Agendado.Application.DTOs;

namespace Agendado.Interface.Service
{
    public interface ICepService
    {
        Task<DadosViaCepResponse?> VerificaCepAsync(string cep);
    }
}

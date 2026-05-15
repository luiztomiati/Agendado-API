using Agendado.Application.Dto;
using Agendado.Application.DTOs;
using Agendado.Domain.Model;
using Agendado.Shared;
using System.Linq.Dynamic.Core;

namespace Agendado.Interface.Service
{
    public interface IUsuarioService
    {
        Task<DadosUsuarioResponse> CriarUsuarioAsync(DadosUsuarioRequest dados);
        Task DeletarUsuarioAsync(Guid usuarioId);
        Task<DadosEditUsuario> EditarUsuarioAsync(Guid usuarioId, DadosEditUsuario dados);
        Task ResetarPasswordAsync(DadosResetarSenhaRequest dados);
        Task ResetarPasswordTokenAsync(DadosResetarPasswordTokenRequest dados);
        Task<DadosResgatarSenhaResponse> ResgatarPasswordAsync(DadosResgatarSenhaRequest dados);
        Task<ResultadoPagincao<DadosUsuarioResponse>> GetUsuariosAsync(int page, int qtdPage);
        Task ConfirmarEmailTokenAsync(string usuarioId,string token);
    }
}

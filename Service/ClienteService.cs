using Agendado.Application.DTOs;
using Agendado.Domain.Model;
using Agendado.Interface.Repository;
using Agendado.Interface.Service;
using Agendado.Repository;
using Agendado.Shared;
using System.Security.Claims;

namespace Agendado.Service
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmpresaRepository _empresaRepository;

        public ClienteService(IClienteRepository clienteRepository, IHttpContextAccessor httpContextAccessor,IUsuarioRepository usuarioRepository, IEmpresaRepository empresaRepository)
        {
            _clienteRepository = clienteRepository;
            _httpContextAccessor = httpContextAccessor;
            _empresaRepository = empresaRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<ResultadoPagincao<DadosCliente>> GetClientesAsync(int page, int qtdPag)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Usuário não encontrado");
            var usuarioLogado = await _usuarioRepository.GetIdentityUserAsync(userId) ?? throw new Exception("Usuário logado não encontrado");

            return await _clienteRepository.ListClientesAsync(usuarioLogado.EmpresaId ,page, qtdPag);
        }
    }
}

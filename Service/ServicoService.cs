using Agendado.Dto;
using Agendado.Interface.Repository;
using Agendado.Interface.Service;
using Agendado.Model;
using System.Security.Claims;

namespace Agendado.Service
{
    public class ServicoService : IServicoService
    {
        private readonly IHttpContextAccessor _httpContextAcessor;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IServicoRepository _servicoRepository;

        public ServicoService(IHttpContextAccessor httpContextAcessor, IUsuarioRepository usuarioRepository, IServicoRepository servicoRepository)
        {
            _httpContextAcessor = httpContextAcessor;
            _usuarioRepository = usuarioRepository;
            _servicoRepository = servicoRepository;
        }

        public async Task CriarServicoAsync(DadosServico dados)
        {
            var userId = _httpContextAcessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Usuário não encontrado");
            var usuarioLogado = await _usuarioRepository.GetIdentityUserAsync(userId) ?? throw new Exception("Usuário logado não encontrado");

            Servicos servico = new Servicos(dados);
            servico.EmpresaId = usuarioLogado.EmpresaId;

            await _servicoRepository.SalvarServicoAsync(servico);
        }
        public async Task EditarServicoAsync(Guid id, DadosServico dados) {
            var servico = await _servicoRepository.GetServicoByIdAsync(id) ?? throw new KeyNotFoundException("Serviço não encontrado");

            servico.Nome = dados.Nome;
            servico.Descricao = dados.Descricao;
            servico.Valor = dados.Valor;
            servico.TempoDuracao = dados.TempoDuracao;
            servico.DtAlteracao = DateTime.UtcNow;

            await _servicoRepository.UpdateServicoAsync(servico);
        }
        public async Task DeletarServicoAsync(Guid id) {
            var servico = await _servicoRepository.GetServicoByIdAsync(id) ?? throw new KeyNotFoundException("Serviço não encontrado");
            await _servicoRepository.DeleteServicoAsync(servico);
           
        }
    }
}

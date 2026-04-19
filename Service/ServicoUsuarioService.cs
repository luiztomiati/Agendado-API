using Agendado.Domain.Model;
using Agendado.Interface.Repository;
using Agendado.Interface.Service;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Agendado.Service
{
    public class ServicoUsuarioService : IServicoUsuarioService
    {
        private readonly IServicoUsuarioRepository _servicoUsuarioRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AgendadoUser> _userManager;
        private readonly IUsuarioRepository _usuarioRepository;

        public ServicoUsuarioService(IServicoUsuarioRepository servicoUsuarioRepository, IHttpContextAccessor httpContextAccessor, UserManager<AgendadoUser> userManager, IUsuarioRepository usuarioRepository)
        {
            _servicoUsuarioRepository = servicoUsuarioRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _usuarioRepository = usuarioRepository;
        }

        public async Task ValidarVinculoUsuarioServicoAsync(Guid usuarioId, Guid servicoId)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Usuário não encontrado");
            var usuarioLogado = await _usuarioRepository.GetIdentityUserAsync(userId) ?? throw new Exception("Usuário não esta autenticado");

            bool existe = await _servicoUsuarioRepository.ExisteServicoUsuarioAsync(usuarioId, servicoId);
            if (!existe) {
                throw new Exception("Conflito de duplicidade: O vínculo entre o usuário e o serviço informado já existe no sistema.");
            }
            var servicoUsuario = new ServicoUsuario(usuarioId, servicoId);

            await _servicoUsuarioRepository.VincularServicoUsuarioAsync(servicoUsuario);
        }
    }
}

using Agendado.Dto;
using Agendado.Interface;
using Agendado.Interface.Repository;
using Agendado.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Agendado.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly UserManager<AgendadoUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAcessor;

        public UsuarioService(IUsuarioRepository usuarioRepository, UserManager<AgendadoUser> userManager, IHttpContextAccessor httpContentAccessor)
        {
           
            _usuarioRepository = usuarioRepository;
            _userManager = userManager;
            _httpContextAcessor = httpContentAccessor;
        }
      
        public DadosUsuarioResponse CriarUsuario(DadosUsuarioRequest dados)
        {
            var userId = _httpContextAcessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Usuário não encontrado");
            var usuarioLogado = _usuarioRepository.GetIdentityUser(userId);

            if (usuarioLogado == null)
                throw new Exception("Apenas usuários SUPER podem criar novos usuários.");

            Usuario usuario = new Usuario(dados);
            var identityUser = new AgendadoUser
            {
                UserName = dados.Email,
                Email = dados.Email
            };


                var result = _userManager.CreateAsync(identityUser, dados.Password)
                                     .GetAwaiter()
                                     .GetResult();

            if (!result.Succeeded)
            {
                throw new Exception("Erro ao criar usuário: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            usuario.IdentityUserId = identityUser.Id;
            usuario.EmpresaId = usuarioLogado.EmpresaId;

            _usuarioRepository.SalvarUsuario(usuario);
            return new DadosUsuarioResponse(usuario.Nome, usuario.Telefone, usuario.DDD, usuario.Email);
        }
        public DadosEditUsuario EditUsuario(Guid usuarioId, DadosEditUsuario dados)
        {

            var usuario = _usuarioRepository.GetUsuario(usuarioId);
            var editUsuario = _usuarioRepository.PutUsuario(usuarioId, dados, usuario);
            
            return new DadosEditUsuario(editUsuario.Nome, editUsuario.Email, editUsuario.Telefone, editUsuario.DDD);
        }
        public void DeleteUsuario(Guid usuarioId)
        {
            _usuarioRepository.DelUsuario(usuarioId);
        }    
    }
}

using Agendado.Dto;
using Agendado.Interface.Repository;
using Agendado.Interface.Service;
using Agendado.Model;
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

        public async Task<DadosUsuarioResponse> CriarUsuarioAsync(DadosUsuarioRequest dados)
        {

            var userId = _httpContextAcessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Usuário não encontrado");
            var usuarioLogado = await _usuarioRepository.GetIdentityUserAsync(userId) ?? throw new Exception("Usuário logado não encontrado"); ;
            var userExists = await _userManager.FindByEmailAsync(dados.Email);

            if (userExists != null)
            {
                throw new Exception("Email já está em uso");
            }

            Usuario usuario = new Usuario(dados);
            var identityUser = new AgendadoUser
            {
                UserName = dados.Email,
                Email = dados.Email
            };

            try
            {

                var result = await _userManager.CreateAsync(identityUser, dados.Password);

                if (!result.Succeeded)
                {
                    throw new Exception("Erro ao criar usuário: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                await _userManager.AddToRoleAsync(identityUser, "USER");
                usuario.IdentityUserId = identityUser.Id;
                usuario.EmpresaId = usuarioLogado.EmpresaId;

                await _usuarioRepository.SalvarUsuarioAsync(usuario);
                return new DadosUsuarioResponse(usuario.Id,usuario.Nome, usuario.Telefone, usuario.DDD, usuario.Email);
            }
            catch (Exception ex)
            {
                await _userManager.DeleteAsync(identityUser);
                throw new Exception(ex.Message);
            }
        }

        public async Task<DadosEditUsuario> EditarUsuarioAsync(Guid usuarioId, DadosEditUsuario dados)
        {
            var userId = _httpContextAcessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new Exception("Usuário não encontrado");

            var usuarioLogado = await _usuarioRepository.GetIdentityUserAsync(userId)
                ?? throw new Exception("Usuário logado não encontrado");

            var isAdmin = _httpContextAcessor.HttpContext.User.IsInRole("ADMIN");

            var usuario = await _usuarioRepository.GetUsuarioByIdAsync(usuarioId)
                ?? throw new Exception("Usuário não encontrado");

            if (usuarioLogado.IdentityUserId != usuario.IdentityUserId && !isAdmin)
            {
                throw new Exception("Usuário sem permissão");
            }

            var identityUser = await _userManager.FindByIdAsync(usuario.IdentityUserId);

            if (identityUser == null)
                throw new Exception("Usuário do Identity não encontrado");

            identityUser.Email = dados.Email;
            identityUser.UserName = dados.Email;

            var emailAntigo = identityUser.Email;
            var userNameAntigo = identityUser.UserName;

            var identityResult = await _userManager.UpdateAsync(identityUser);
            if (!identityResult.Succeeded)
            {
                throw new Exception("Erro ao atualizar usuário do Identity: " +
                    string.Join(", ", identityResult.Errors.Select(e => e.Description)));
            }
            try
            {
                usuario.Nome = dados.Nome;
                usuario.DDD = dados.DDD;
                usuario.Telefone = dados.Telefone;
                usuario.Email = dados.Email;

                var editUsuario = await _usuarioRepository.UpdateUsuarioAsync(usuario);

                return new DadosEditUsuario(
                    editUsuario.Nome,
                    editUsuario.Email,
                    editUsuario.Telefone,
                    editUsuario.DDD
                );
            }
            catch (Exception ex)
            {
                identityUser.Email = emailAntigo;
                identityUser.UserName = userNameAntigo;

                await _userManager.UpdateAsync(identityUser);
                throw new Exception(ex.Message);
            }
        }
        public async Task DeletarUsuarioAsync(Guid usuarioId)
        {
            var usuario = await _usuarioRepository.GetUsuarioByIdAsync(usuarioId) ?? throw new Exception("Usuário não encontrado");
            await _usuarioRepository.DeleteUsuarioAsync(usuario);
        }    
    }
}

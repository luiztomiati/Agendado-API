using Agendado.Dto;
using Agendado.Interface.Repository;
using Agendado.Interface.Service;
using Agendado.Model;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;

namespace Agendado.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly UserManager<AgendadoUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAcessor;
        private readonly EmailSettings _emailSettings;

        public UsuarioService(IUsuarioRepository usuarioRepository, UserManager<AgendadoUser> userManager, IHttpContextAccessor httpContentAccessor, EmailSettings emailSettings)
        {

            _usuarioRepository = usuarioRepository;
            _userManager = userManager;
            _httpContextAcessor = httpContentAccessor;
            _emailSettings = emailSettings;
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
                return new DadosUsuarioResponse(usuario.Id, usuario.Nome, usuario.Telefone, usuario.DDD, usuario.Email);
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

        public async Task ResetarPasswordAsync(DadosResetarSenhaRequest dados)
        {
            var userId = _httpContextAcessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Usuário não encontrado");
            var usuarioLogado = await _userManager.FindByIdAsync(userId) ?? throw new Exception("Usuário logado não localizado");

            var result = await _userManager.ChangePasswordAsync(usuarioLogado, dados.PasswordAtual, dados.NovoPassword);

            if (!result.Succeeded)
            {
                throw new Exception(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );
            }
        }
        public async Task<DadosResgatarSenhaResponse> ResgatarPasswordAsync(DadosResgatarSenhaRequest dados)
        {
            var user = await _userManager.FindByEmailAsync(dados.Email);

            if (user == null)
            {
                return new DadosResgatarSenhaResponse
                {
                    EmailEnviado = false
                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            string corpoHtml = $@"
    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; text-align: center; border: 1px solid #eee; padding: 20px;'>
        <h2 style='color: #333;'>Redefinição de Senha</h2>
        <p style='color: #666;'>Você solicitou a recuperação de senha para sua conta no <strong>Agendado</strong>.</p>
        <p style='color: #666;'>Clique no botão abaixo para escolher uma nova senha:</p>
        
        <div style='margin: 30px 0;'>
            <a href='{token}' 
               style='background-color: #007bff; color: white; padding: 15px 25px; text-decoration: none; border-radius: 5px; font-weight: bold; display: inline-block;'>
               Redefinir Minha Senha
            </a>
        </div>
        
        <p style='font-size: 12px; color: #999;'>Se o botão não funcionar, copie e cole o link abaixo no seu navegador:</p>
        <p style='font-size: 12px; color: #007bff; word-break: break-all;'>{token}</p>
        
        <hr style='border: 0; border-top: 1px solid #eee; margin: 20px 0;' />
        <p style='font-size: 12px; color: #999;'>Se você não solicitou isso, pode ignorar este e-mail com segurança.</p>
    </div>";

            await _emailSettings.EnviarEmailAsync(new List<string> { user.Email }, "Esqueci minha senha", corpoHtml, null);
            return new DadosResgatarSenhaResponse { EmailEnviado = true, Token = token };
        }
        public async Task ResetarPasswordTokenAsync(DadosResetarPasswordTokenRequest dados)
        {
            try
            {
                var usuario = await _userManager.FindByEmailAsync(dados.Email) ?? throw new Exception("Usuário não foi encontrado");
                var result = await _userManager.ResetPasswordAsync(usuario, dados.Token, dados.NovoPassword);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

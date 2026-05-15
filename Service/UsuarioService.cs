using Agendado.Application.Dto;
using Agendado.Application.DTOs;
using Agendado.Domain.Entities;
using Agendado.Domain.Model;
using Agendado.Interface.Repository;
using Agendado.Interface.Service;
using Agendado.Shared;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Agendado.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly UserManager<AgendadoUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAcessor;
        private readonly IEmailService _emailSettings;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IAtendimentoService _atendimentoService;
        private readonly IEmpresaAtendimentoRepository _empresaAtendimentoRepository;
        private readonly IAtendimentoRepository _atendimentoRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository, UserManager<AgendadoUser> userManager, IHttpContextAccessor httpContentAccessor, IEmailService emailSettings, IEmpresaRepository empresaRepository, IAtendimentoService atendimentoService, IEmpresaAtendimentoRepository empresaAtendimentoRepository, IAtendimentoRepository atendimentoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _userManager = userManager;
            _httpContextAcessor = httpContentAccessor;
            _emailSettings = emailSettings;
            _empresaRepository = empresaRepository;
            _atendimentoService = atendimentoService;
            _empresaAtendimentoRepository = empresaAtendimentoRepository;
            _atendimentoRepository = atendimentoRepository;
        }
        public async Task<DadosUsuarioResponse> CriarUsuarioAsync(DadosUsuarioRequest dados)
        {
            var userId = _httpContextAcessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Usuário não encontrado");
            var usuarioLogado = await _usuarioRepository.GetIdentityUserAsync(userId) ?? throw new Exception("Usuário logado não encontrado");
            var userExists = await _userManager.FindByEmailAsync(dados.Email);

            if (userExists != null)
            {
                throw new Exception("Email já está em uso");
            }

            var identityUser = new AgendadoUser
            {
                UserName = dados.Email,
                Email = dados.Email,
                PrimeiroLogin = true,
            };
            Usuario usuario = new Usuario(dados.Nome,dados.DDD, dados.Telefone,dados.Email,usuarioLogado.EmpresaId, identityUser.Id);

            try
            {
                var password = GerarSenha();
                var result = await _userManager.CreateAsync(identityUser, password);

                if (!result.Succeeded)
                {
                    throw new Exception("Erro ao criar usuário: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                await _userManager.AddToRoleAsync(identityUser, "USER");
                await _atendimentoService.HorarioAtendimentoPadrao(usuario);
                await _usuarioRepository.SalvarUsuarioAsync(usuario);
                await EnviarConfirmacaoEmail(identityUser);

                return new DadosUsuarioResponse(usuario.Id, usuario.Nome, usuario.Email, usuario.DDD, usuario.Telefone);
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
            var usuarioIdentity = await _userManager.FindByIdAsync(usuario.IdentityUserId.ToString()) ?? throw new Exception("Conta de acesso não localizada no Identity");
            await _userManager.DeleteAsync(usuarioIdentity);
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
                if (usuario.PrimeiroLogin == true) {
                    usuario.PrimeiroLogin = false;
                    await _usuarioRepository.UpdateUsuarioPrimeiroLoginAsync(usuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResultadoPagincao<DadosUsuarioResponse>> GetUsuariosAsync(int page, int qtdPage)
        {
            var userId = _httpContextAcessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Usuário não encontrado");
            var usuarioLogado = await _usuarioRepository.GetIdentityUserAsync(userId) ?? throw new Exception("Usuário logado não encontrado");

            var usuarios = await _usuarioRepository.GetUsuariosAsync(usuarioLogado.EmpresaId, page, qtdPage);

            return usuarios;
        }

        private static string GerarSenha(int tamanho = 12)
        {
            const string minusculas = "abcdefghijkmnopqrstuvwxyz";
            const string maiusculas = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            const string numeros = "0123456789";
            const string especiais = "!@$?_-%#&*+";

            string todosOsCaracteres = minusculas + maiusculas + numeros + especiais;
            
            var min = PegarCaracterAleatorio(minusculas);
            var mai = PegarCaracterAleatorio(maiusculas);
            var num = PegarCaracterAleatorio(numeros);
            var esp = PegarCaracterAleatorio(especiais);

            var senha = new char[tamanho];
            var data = new byte[tamanho];

            senha[0] = min;
            senha[1] = mai;
            senha[2] = num;
            senha[3] = esp;

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);

                for (int i = 4; i < tamanho; i++)
                {
                    int index = data[i] % todosOsCaracteres.Length;
                    senha[i] = todosOsCaracteres[index];
                }
            }
            return Embaralhar(senha);
        }

        private async Task PrimeiroAcesso(AgendadoUser userIdentity)
        {
            var user = await _userManager.FindByEmailAsync(userIdentity.Email);

            if (user == null)
            {
                throw new Exception("Usuário não encontrado");
            }
            
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var usuario = await _usuarioRepository.GetIdentityUserAsync(user.Id);

            var empresa = await _empresaRepository.GetEmpresaByIdAsync(usuario.EmpresaId);
            string corpoHtml = $@"
<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; text-align: center; border: 1px solid #eee; padding: 20px;'>
    <h2 style='color: #333;'>Bem-vindo ao Agendado!</h2>
    <p style='color: #666;'>Sua conta foi criada com sucesso pelo administrador da {empresa.Nome}</p>
    <p style='color: #666;'>Para começar a utilizar o sistema, você precisa <strong>ativar seu acesso</strong> e definir sua senha pessoal:</p>
    
    <div style='margin: 30px 0;'>
        <a href='{token}' 
            style='background-color: #007bff; color: white; padding: 15px 25px; text-decoration: none; border-radius: 5px; font-weight: bold; display: inline-block;'>
            Ativar Minha Conta e Definir Senha
        </a>
    </div>
    
    <p style='font-size: 12px; color: #999;'>Se o botão não funcionar, utilize o token abaixo na sua aplicação:</p>
    <p style='font-size: 12px; color: #007bff; word-break: break-all;'>{token}</p>
    
    <hr style='border: 0; border-top: 1px solid #eee; margin: 20px 0;' />
    <p style='font-size: 12px; color: #999;'>Este é um convite de acesso. Se você não conhece esta empresa, ignore este e-mail.</p>
</div>";

            await _emailSettings.EnviarEmailAsync(new List<string> { user.Email}, $"Bem-vindo ao Agendado", corpoHtml, null);
        }

        private static char PegarCaracterAleatorio(string caracteres)
        {
            var index = RandomNumberGenerator.GetInt32(caracteres.Length);
            return caracteres[index];
        }
        private static string Embaralhar(char[] senha)
        {
            for (int i = senha.Length - 1; i > 0; i--)
            {
                int j = RandomNumberGenerator.GetInt32(i + 1);

                (senha[i], senha[j]) = (senha[j], senha[i]);
            }
            return new string(senha);
        }

        private async Task EnviarConfirmacaoEmail(AgendadoUser identityUser)
        {
            var user = await _userManager.FindByEmailAsync(identityUser.Email) ?? throw new Exception("Usuário não encontrado");

            var usuario = await _usuarioRepository.GetIdentityUserAsync(user.Id.ToString()) ?? throw new Exception("Usuário não encontrado");
            
            var empresa = await _empresaRepository.GetEmpresaByIdAsync(usuario.EmpresaId);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);  

            var baseUrl = Environment.GetEnvironmentVariable("BASE__URL");

            var linkConfirmacao = $"{baseUrl}/api/Usuario/confirmar-email?usuarioId={user.Id}&token={token}";

            string body = @$"
<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; text-align: center; border: 1px solid #eee; padding: 20px;'>
    <h2 style='color: #333;'>Confirme seu e-mail</h2>
    
    <p style='color: #666;'>Olá!</p>
    
    <p style='color: #666;'>
        Você está quase pronto para começar a usar o sistema da <strong>{empresa?.Nome}</strong>.
    </p>
    
    <p style='color: #666;'>
        Para garantir a segurança da sua conta, precisamos que você confirme seu endereço de e-mail:
    </p>
    
    <div style='margin: 30px 0;'>
        <a href='{linkConfirmacao}' 
           style='background-color: #28a745; color: white; padding: 15px 25px; text-decoration: none; border-radius: 5px; font-weight: bold; display: inline-block;'>
            Confirmar Meu E-mail
        </a>
    </div>
  
    <p style='font-size: 12px; color: #999;'>
        Este link pode expirar por motivos de segurança.
    </p>
    
    <hr style='border: 0; border-top: 1px solid #eee; margin: 20px 0;' />
    
    <p style='font-size: 12px; color: #999;'>
        Se você não solicitou este e-mail, pode ignorá-lo com segurança.
    </p>
</div>";
            await _emailSettings.EnviarEmailAsync(new List<string> { usuario.Email }, "Confirmação de Email", body, null);
        }

        public async Task ConfirmarEmailTokenAsync(string usuarioId,string token)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(usuarioId) ?? throw new Exception("Usuário não encontrado");
                await _userManager.ConfirmEmailAsync(user, token);
                if (user.PrimeiroLogin)
                {
                    await PrimeiroAcesso(user);
                }
            } catch (Exception) {
                throw new Exception("Erro ao confirmar email");
            }
        }
    }
}

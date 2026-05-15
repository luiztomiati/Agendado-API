using Agendado.Application.Dto;
using Agendado.Data;
using Agendado.Domain.Model;
using Agendado.Interface.Repository;
using Agendado.Interface.Service;
using Microsoft.AspNetCore.Identity;
using Sprache;

namespace Agendado.Service
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly UserManager<AgendadoUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ICepService _cepService;
        private readonly IAtendimentoService _atendimentoService;
        private readonly IEmailService _emailSettings;

        public EmpresaService(IUsuarioRepository usuarioRepository, IEmpresaRepository empresaRepository, AppDbContext context, UserManager<AgendadoUser> userManager, ICepService cepService, IAtendimentoService atendimentoService, IEmailService emailSettings)
        {
            _usuarioRepository = usuarioRepository;
            _empresaRepository = empresaRepository;
            _context = context;
            _userManager = userManager;
            _cepService = cepService;
            _atendimentoService = atendimentoService;
            _emailSettings = emailSettings;
        }

        public async Task CriarEmpresaUsuarioAdminAsync(DadosEmpresaUsuarioAdmin dados)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var viaCep = await _cepService.VerificaCepAsync(dados.Cep) ?? throw new Exception("Cep não foi localizado");
                Empresa empresa = new Empresa(dados.NomeEmpresa,dados.Numero,dados.Cep,viaCep.Localidade,viaCep.Uf,viaCep.Logradouro,viaCep.Bairro);
                await _atendimentoService.CriarHorarioAtendimentoEmpresa(dados.Horarios, empresa);

                var identityUser = new AgendadoUser
                {
                    UserName = dados.Email,
                    Email = dados.Email,
                    PhoneNumber = dados.Telefone,
                    PrimeiroLogin = false
                };
                var result = await _userManager.CreateAsync(identityUser, dados.Password);

                if (!result.Succeeded)
                {
                    throw new Exception("Erro ao criar usuário: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                await _userManager.AddToRoleAsync(identityUser, "ADMIN");

                Usuario usuario = new(dados.NomeUsuario, dados.DDD, dados.Telefone, dados.Email, empresa.Id, identityUser.Id);
                await _atendimentoService.CriarAtendimentoUsuario(empresa, usuario);

                _usuarioRepository.CriarUsuario(usuario);
                _empresaRepository.CriarEmpresa(empresa);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                await EnviarConfirmacaoEmailEmpresa(identityUser);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        private async Task EnviarConfirmacaoEmailEmpresa(AgendadoUser identityUser)
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
        Você acaba de realizar o cadastro da empresa: <strong>{empresa?.Nome}</strong>.
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
    }
}

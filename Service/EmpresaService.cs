using Agendado.Data;
using Agendado.Dto;
using Agendado.Interface;
using Agendado.Model;
using Microsoft.AspNetCore.Identity;
using Sprache;

namespace Agendado.Service
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;

        public EmpresaService(IUsuarioRepository usuarioRepository, IEmpresaRepository empresaRepository, AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _usuarioRepository = usuarioRepository;
            _empresaRepository = empresaRepository;
            _context = context;
            _userManager = userManager;
        }

        // 1. Mudamos de 'void' para 'async Task'
        public void CriarEmpresaUsuarioAdmin(DadosEmpresaUsuarioAdmin dados)
        {
            try
            {
                Empresa empresa = new Empresa { Nome = dados.NomeEmpresa };

                var identityUser = new IdentityUser
                {
                    UserName = dados.Email,
                    Email = dados.Email
                };

                // .GetAwaiter().GetResult() é a forma mais segura de forçar sincronismo
                var result = _userManager.CreateAsync(identityUser, dados.Password)
                                         .GetAwaiter()
                                         .GetResult();

                if (!result.Succeeded)
                {
                    throw new Exception("Erro ao criar usuário: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                Usuario usuario = new Usuario
                {
                    Nome = dados.NomeUsuario,
                    DDD = dados.DDD,
                    Telefone = dados.Telefone,
                    Email = dados.Email,
                    Empresa = empresa,
                    Role = Enums.Role.SUPER,
                    IdentityUserId = identityUser.Id
                };

                empresa.SuperUsuarioId = usuario.Id;

                _empresaRepository.CriarEmpresa(empresa);
                _usuarioRepository.CriarUsuario(usuario);

                // Salva as alterações de forma síncrona
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                // Use 'throw;' para preservar a stack trace original do erro
                throw;
            }
        }
    }
}

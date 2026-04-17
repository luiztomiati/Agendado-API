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

        public EmpresaService(IUsuarioRepository usuarioRepository, IEmpresaRepository empresaRepository, AppDbContext context, UserManager<AgendadoUser> userManager)
        {
            _usuarioRepository = usuarioRepository;
            _empresaRepository = empresaRepository;
            _context = context;
            _userManager = userManager;
        }

        public async Task CriarEmpresaUsuarioAdminAsync(DadosEmpresaUsuarioAdmin dados)
        {
            try
            {
                Empresa empresa = new Empresa { Nome = dados.NomeEmpresa };

                var identityUser = new AgendadoUser
                {
                    UserName = dados.Email,
                    Email = dados.Email
                };

                var result = await _userManager.CreateAsync(identityUser, dados.Password);

                if (!result.Succeeded)
                {
                    throw new Exception("Erro ao criar usuário: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                await _userManager.AddToRoleAsync(identityUser, "ADMIN");

                Usuario usuario = new Usuario
                {
                    Nome = dados.NomeUsuario,
                    DDD = dados.DDD,
                    Telefone = dados.Telefone,
                    Email = dados.Email,
                    Empresa = empresa,
                    IdentityUserId = identityUser.Id
                };

                _empresaRepository.CriarEmpresa(empresa);
                _usuarioRepository.CriarUsuario(usuario);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

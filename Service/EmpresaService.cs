using Agendado.Application.Dto;
using Agendado.Application.DTOs;
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

        public EmpresaService(IUsuarioRepository usuarioRepository, IEmpresaRepository empresaRepository, AppDbContext context, UserManager<AgendadoUser> userManager, ICepService cepService)
        {
            _usuarioRepository = usuarioRepository;
            _empresaRepository = empresaRepository;
            _context = context;
            _userManager = userManager;
            _cepService = cepService;
        }

        public async Task CriarEmpresaUsuarioAdminAsync(DadosEmpresaUsuarioAdmin dados)
        {
            try
            {
                var viaCep = await _cepService.VerificaCepAsync(dados.Cep);

                if (viaCep == null) throw new Exception("Cep não foi localizado");

                Empresa empresa = new Empresa(dados.NomeEmpresa,dados.Numero,dados.Cep,viaCep.Localidade,viaCep.Uf,viaCep.Logradouro,viaCep.Bairro);

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

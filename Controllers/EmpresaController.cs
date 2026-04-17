using Agendado.Application.Dto;
using Agendado.Interface.Service;
using Agendado.Service;
using Microsoft.AspNetCore.Mvc;

namespace Agendado.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaService _empresaService;

        public EmpresaController(IEmpresaService empresaService)
        {
            _empresaService = empresaService;
        }

        [HttpPost("criar")]
        public async Task<IActionResult> CriarEmpresaUsuarioAdmin(DadosEmpresaUsuarioAdmin dados)
        {
            try
            {
                await _empresaService.CriarEmpresaUsuarioAdminAsync(dados);
                return Ok();
            }
            catch (Exception ex) { 
                return StatusCode(500, ex.Message);
            }
        }
    }
}

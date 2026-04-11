using Agendado.Dto;
using Agendado.Interface.Repository;
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

        [HttpPost]
        public IActionResult CriarEmpresaUsuarioAdmin(DadosEmpresaUsuarioAdmin dados)
        {
            try
            {
                _empresaService.CriarEmpresaUsuarioAdmin(dados);
                return Ok();
            }
            catch (Exception ex) { 
                return StatusCode(500, ex.Message);
            }
        }
    }
}

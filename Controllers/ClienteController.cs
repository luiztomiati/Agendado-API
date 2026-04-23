using Agendado.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agendado.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("clientes/{page}/{qtdPag}")]
        [Authorize(Policy = "OnboardingConcluido")]
        public async Task<IActionResult> GetClientes(int page, int qtdPag)
        {
            try
            {
                var clientes = await _clienteService.GetClientesAsync(page, qtdPag);
                return Ok(clientes);
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }
        }
    }
}

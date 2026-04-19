using Agendado.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agendado.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ServicoUsuario : ControllerBase
    {
        private readonly IServicoUsuarioService _servicoUsuarioService;

        public ServicoUsuario(IServicoUsuarioService servicoUsuarioService)
        {
            _servicoUsuarioService = servicoUsuarioService;
        }

        [HttpPost("usuarios/{usuarioId}/servicos/{servicoId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> PostServicoUsuario(Guid usuarioId, Guid servicoId)
        {
            try
            {
                await  _servicoUsuarioService.ValidarVinculoUsuarioServicoAsync(usuarioId, servicoId);
                return Ok();
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }
    }
}

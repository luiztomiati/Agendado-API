using Agendado.Dto;
using Agendado.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agendado.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        
        public IActionResult CriarUsuario(DadosUsuarioRequest dados)
        {
            var usuario = _usuarioService.CriarUsuario(dados);
            return Ok(usuario);
        }
        [HttpPut]
        public IActionResult EditUsuario(Guid usuarioId, DadosEditUsuario dados)
        {
            var usuario = _usuarioService.EditUsuario(usuarioId, dados);
            return Ok(usuario);
        }
        [HttpDelete]
        public IActionResult DeleteUsuario(Guid usuarioId)
        {
            try
            {
                _usuarioService.DeleteUsuario(usuarioId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

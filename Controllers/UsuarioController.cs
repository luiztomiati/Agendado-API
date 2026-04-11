using Agendado.Dto;
using Agendado.Interface.Repository;
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
        [Authorize(Roles = "ADMIN")]
        public IActionResult CriarUsuario(DadosUsuarioRequest dados)
        {
            var usuario = _usuarioService.CriarUsuario(dados);
            return Ok(usuario);
        }
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public IActionResult EditUsuario(Guid usuarioId, DadosEditUsuario dados)
        {
            var usuario = _usuarioService.EditUsuario(usuarioId, dados);
            return Ok(usuario);
        }
        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
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

using Agendado.Dto;
using Agendado.Interface.Service;
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

        [HttpPost("criar")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> PostUsuario(DadosUsuarioRequest dados)
        {
            try
            {
                var usuario = await _usuarioService.CriarUsuarioAsync(dados);
                return Ok(usuario);
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);            
            }
        }
        [HttpPut("editar")]
        [Authorize(Roles = "ADMIN, USER")]
        public async Task<IActionResult> PutUsuario(Guid usuarioId, DadosEditUsuario dados)
        {
            try
            {
                var usuario = await _usuarioService.EditarUsuarioAsync(usuarioId, dados);
                return Ok(usuario);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
          
        }
        [HttpDelete("deletar")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteUsuario(Guid usuarioId)
        {
            try
            {
                await _usuarioService.DeletarUsuarioAsync(usuarioId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("resetar-password")]
        [Authorize(Roles = "ADMIN, USER")]
        public async Task<IActionResult> ResetarPassword(DadosResetarSenhaRequest dados)
        {
            try
            {
                await _usuarioService.ResetarPasswordAsync(dados);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

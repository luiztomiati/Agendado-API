using Agendado.Application.Dto;
using Agendado.Interface.Repository;
using Agendado.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Agendado.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private IUsuarioService _usuarioService;
        private IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioService usuarioService, IUsuarioRepository usuarioRepository)
        {
            _usuarioService = usuarioService;
            _usuarioRepository = usuarioRepository;
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
        [HttpPut("editar/{id}")]
        [Authorize(Roles = "ADMIN, USER")]
        public async Task<IActionResult> PutUsuario(Guid id, DadosEditUsuario dados)
        {
            try
            {
                var usuario = await _usuarioService.EditarUsuarioAsync(id, dados);
                return Ok(usuario);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
          
        }
        [HttpDelete("deletar/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteUsuario(Guid id)
        {
            try
            {
                await _usuarioService.DeletarUsuarioAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("resetar-password")]
        [EnableRateLimiting("reset-password")]
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
        [HttpPost("esqueci-minha-senha")]
        [EnableRateLimiting("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> EsqueciMinhaSenha(DadosResgatarSenhaRequest dados)
        {
            try
            {
                var result = await _usuarioService.ResgatarPasswordAsync(dados);
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("resetar-password-token")]
        [EnableRateLimiting("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetarPasswordToken(DadosResetarPasswordTokenRequest dados)
        {
            try
            {
                await _usuarioService.ResetarPasswordTokenAsync(dados);
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-usuarios")]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<IActionResult> GetUsuarios(int page, int qtdPag)
        {
            try
            {
                var usuarios = await _usuarioService.GetUsuariosAsync(page, qtdPag);
                return Ok(usuarios);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

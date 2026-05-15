using Agendado.Application.Dto;
using Agendado.Application.DTOs;
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
        [Authorize(Policy = "OnboardingConcluido")]
        public async Task<IActionResult> PostUsuario(DadosUsuarioRequest dados)
        {
            try
            {
                var usuario = await _usuarioService.CriarUsuarioAsync(dados);
                return Ok(usuario);
            }
            catch (Exception) 
            { 
                return BadRequest("Não foi possivel criar o usuário.");            
            }
        }
        [HttpPut("editar/{id}")]
        [Authorize(Roles = "ADMIN, USER")]
        [Authorize(Policy = "OnboardingConcluido")]
        public async Task<IActionResult> PutUsuario(Guid id, DadosEditUsuario dados)
        {
            try
            {
                var usuario = await _usuarioService.EditarUsuarioAsync(id, dados);
                return Ok(usuario);

            }catch(Exception)
            {
                return BadRequest("Não foi possivel editar o usuário.");
            }
          
        }
        [HttpDelete("deletar/{id}")]
        [Authorize(Roles = "ADMIN")]
        [Authorize(Policy = "OnboardingConcluido")]
        public async Task<IActionResult> DeleteUsuario(Guid id)
        {
            try
            {
                await _usuarioService.DeletarUsuarioAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound("Não foi possível deletar o usuário.");
            }
        }

        [HttpPut("resetar-password")]
        [EnableRateLimiting("reset-password")]
        [Authorize(Roles = "ADMIN, USER")]
        [Authorize(Policy = "OnboardingConcluido")]
        public async Task<IActionResult> ResetarPassword(DadosResetarSenhaRequest dados)
        {
            try
            {
                await _usuarioService.ResetarPasswordAsync(dados);
                return Ok("Senha resetada com sucesso");
            }
            catch (Exception)
            {
                return BadRequest("Não foi possivel resetar a senha.");
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
                return Ok("Se o Email estiver cadastrado na nossa base de dados, você receberá instruções para redefinir sua senha.");
            }catch(Exception)
            {
                return BadRequest("Não foi possível processar sua solicitação no momento.");
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
                return Ok("Senha resetada com Sucesso");
            }catch(Exception)
            {
                return BadRequest("Não foi possivel resetar a senha.");
            }
        }

        [HttpGet("get-usuarios")]
        [Authorize(Roles = "ADMIN, USER")]
        [Authorize(Policy = "OnboardingConcluido")]
        public async Task<IActionResult> GetUsuarios(int page, int qtdPag)
        {
            try
            {
                var usuarios = await _usuarioService.GetUsuariosAsync(page, qtdPag);
                return Ok(usuarios);

            }catch(Exception)
            {
                return BadRequest("Erro ao buscar os usuários.");
            }
        }

        [HttpGet("get-usuarioId/{usuarioId}")]
        [Authorize(Roles = "ADMIN, USER")]
        [Authorize(Policy = "OnboardingConcluido")]
        public async Task<IActionResult> GetUsuarioById(Guid usuarioId)
        {
            try
            {
                var usuario = await _usuarioRepository.GetUsuarioByIdAsync(usuarioId);
                return Ok(usuario);
            }catch(Exception)
            {
                return BadRequest("Erro ao buscar os usuário.");
            }
        }

        [HttpGet("confirmar-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmarEmail(string usuarioId, string token) {
            try {
                await _usuarioService.ConfirmarEmailTokenAsync(usuarioId, token);
                return Ok("Confirmação realizada com sucesso.");
            }catch(Exception) {
                return BadRequest("Erro na confirmação do Email.");
            
            }
        }
    }
}

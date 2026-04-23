using Agendado.Application.Dto;
using Agendado.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agendado.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ServicosController : ControllerBase
    {
        private readonly IServicoService _servicoService;

        public ServicosController(IServicoService servicoService)
        {
            _servicoService = servicoService;
        }

        [HttpPost("criar")]
        [Authorize(Roles = "ADMIN")]
        [Authorize(Policy = "OnboardingConcluido")]
        public async Task<IActionResult> PostServico(DadosServico dados)
        {
            try
            {
                await _servicoService.CriarServicoAsync(dados);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPut("editar/{id}")]
        [Authorize(Roles = "ADMIN")]
        [Authorize(Policy = "OnboardingConcluido")]
        public async Task<IActionResult> PutServico(Guid id, DadosServico dados)
        {
            try
            {
                await _servicoService.EditarServicoAsync(id, dados);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("deletar/{id}")]
        [Authorize(Roles = "ADMIN")]
        [Authorize(Policy = "OnboardingConcluido")]
        public async Task<IActionResult> DeleteServico(Guid id)
        {
            try
            {
                await _servicoService.DeletarServicoAsync(id);
                return Ok();

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-servicoId/{servicoId}")]
        [Authorize(Roles = "ADMIN, USER")]
        [Authorize(Policy = "OnboardingConcluido")]
        public async Task<IActionResult> GetServicoById(Guid servicoId)
        {
            try 
            {
                var servico = await _servicoService.GetServicoByIdAsync(servicoId);
                return Ok(servico);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-servicos")]
        [Authorize(Roles = "ADMIN, USER")]
        [Authorize(Policy = "OnboardingConcluido")]
        public async Task<IActionResult> GetServicos(int page, int qtdPag)
        {
            try
            {
                var servico = await _servicoService.ListServicosAsync(page, qtdPag);
                return Ok(servico);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

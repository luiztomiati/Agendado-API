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
        [HttpPut("editar")]
        [Authorize(Roles = "ADMIN")]
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
        [HttpDelete("deletar")]
        [Authorize(Roles = "ADMIN")]
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

    }
}

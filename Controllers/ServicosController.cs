using Agendado.Application.Dto;
using Agendado.Domain.Model;
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

        //[HttpPost("criar")]
        //[Authorize(Roles = "ADMIN")]
        //[Authorize(Policy = "OnboardingConcluido")]
        //public async Task<IActionResult> PostServico(DadosServico dados)
        //{
        //    try
        //    {
        //        var servico = await _servicoService.CriarServicoAsync(dados);
        //        return CreatedAtAction(nameof(GetServicoById) new {id = servico.Id} servico);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, new { mensagem = "Não foi possível criar o serviço." });
        //    }

        //}
        [HttpPut("editar/{id}")]
        [Authorize(Roles = "ADMIN")]
        [Authorize(Policy = "OnboardingConcluido")]
        public async Task<IActionResult> PutServico(Guid id, DadosServico dados)
        {
            try
            {
                await _servicoService.EditarServicoAsync(id, dados);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensagem = "Não foi possível editar o serviço." });
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

            }catch(Exception)
            {
                return BadRequest(new { Menssagem = "Não foi possivel deletar o serviço" });
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
            }catch(Exception)
            {
                return BadRequest(new { Menssagem = "Erro ao buscar os serviços"});
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
            }catch(Exception)
            {
                return BadRequest(new { Menssagem = "Erro ao buscar o serviço" });
            }
        }
    }
}

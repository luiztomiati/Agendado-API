using Agendado.Application.DTOs;
using Agendado.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agendado.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AtendimentoController : ControllerBase
    {
        private readonly IAtendimentoService _atendimentoService;

        public AtendimentoController(IAtendimentoService atendimentoService)
        {
            _atendimentoService = atendimentoService;
        }

        [HttpPut("editar-horario-atendimento")]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<IActionResult> EditarHorarioAtendimento(UsuarioHorarioAtendimentoDTO dados)
        {
            await _atendimentoService.EditarHorarioAtendimento(dados);
            return Ok("Edição realizada com sucesso.");
        }
    }
}

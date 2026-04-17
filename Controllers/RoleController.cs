using Agendado.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Agendado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AgendadoUser> _userManager;
    
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AgendadoUser> userMananger)
        {
            _roleManager = roleManager;
            _userManager = userMananger;
        }

        [HttpPost("registrar-role")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RegistrarRoleAsync(string papel)
        {
            var newRole = await _roleManager.FindByNameAsync(papel);
            if (newRole is not null)
            {
                return BadRequest("Já foi registrada na base de dados.");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(papel));
            if (!result.Succeeded)
            {
                return BadRequest($"Falha ao registrar.");
            }
            return Ok(new { Mensagem = "Registrado com sucesso." });
        }

        [HttpPost("atribuir-role")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AtribuirRoleAsync(string email, string papel)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return BadRequest("Usuário não encontrado.");
            }

            var role = await _roleManager.FindByNameAsync(papel);
            if (role is null)
            {
                return BadRequest("Role não encontrado.");
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name!);

            if (!result.Succeeded)
            {
                return BadRequest($"Falha ao atribuir role.");
            }

            return Ok(new { Mensagem = "Role atribuido com sucesso." });
        }

    }
}

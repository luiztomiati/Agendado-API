using Agendado.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Agendado.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class RolesController : ControllerBase
        {
            private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AgendadoUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AgendadoUser> userMananger)
            {
                _roleManager = roleManager;
                _userManager = userMananger;
            }

        [HttpPost("registrar-papel")]
        public async Task<IActionResult> RegistrarRoleAsync(string papel)
        {
            var newRole = await _roleManager.FindByNameAsync(papel);
            if (newRole is not null)
            {
                return BadRequest("Papel/Cargo já foi registrada na base de dados.");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(papel));
            if (!result.Succeeded)
            {
                return BadRequest($"Falha ao registrar Papel/Cargo.");
            }
            return Ok(new { Mensagem = "Papel/Cargo registrada com sucesso." });
        }

        [HttpPost("atribuir-papel")]
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
                return BadRequest("Papel/Cargo não encontrado.");
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name!);

            if (!result.Succeeded)
            {
                return BadRequest($"Falha ao atribuir Papel/Cargo.");
            }

            return Ok(new { Mensagem = "Papel/Cargo atribuido com sucesso." });
        }
    }
}

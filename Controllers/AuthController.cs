using Agendado.Dto;
using Agendado.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Agendado.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly TokenService _tokenService;

        public AuthController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager,TokenService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(DadosLogin usuario)
        {
            var result = await _signInManager.PasswordSignInAsync(usuario.Email, usuario.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return BadRequest("Falha no login do usuário.");
            }
            return Ok(new {message = "Uuário logado com sucesso", token = _tokenService.GerarTokenDeUsuario(usuario)});
        }
    }
}

using Agendado.Dto;
using Agendado.Model;
using Agendado.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Agendado.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AgendadoUser> _userManager;
        private readonly SignInManager<AgendadoUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _config;

        public AuthController(UserManager<AgendadoUser> userManager,SignInManager<AgendadoUser> signInManager,TokenService tokenService, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _config = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(DadosLogin usuario)
        {
            var user = await _userManager.FindByEmailAsync(usuario.Email!) ?? throw new Exception("Usuário não foi encontrado");
            
            var result = await _signInManager.PasswordSignInAsync(usuario.Email, usuario.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return BadRequest("Falha no login do usuário.");
            }
            
            DadosUsuarioToken tokenDto = await _tokenService.GerarTokenDeUsuario(user);
            var refreshToken = _tokenService.GerarRefreshToken();
            tokenDto.RefreshToken = refreshToken;

            
            user.RefreshToken = refreshToken;
            var expire = int.TryParse(_config["JWTTokenConfiguration:RefreshExpireInMinutes"],
                out int refreshExpireInMinutes);
            user.TempoExpiracao = DateTime.UtcNow.AddMinutes(refreshExpireInMinutes);
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                message = "Uuário logado com sucesso",
                token = tokenDto
            });
        }
    }
}

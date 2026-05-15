using Agendado.Application.Dto;
using Agendado.Domain.Model;
using Agendado.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        [EnableRateLimiting("reset-password")]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(DadosLogin usuario)
        {
            var user = await _userManager.FindByEmailAsync(usuario.Email!) ?? throw new Exception("Usuário não foi encontrado");
            
            var result = await _signInManager.CheckPasswordSignInAsync(user, usuario.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {

                return BadRequest("Falha no login do usuário.");
            }
            
            DadosUsuarioToken tokenDto = await _tokenService.GerarTokenDeUsuarioAsync(user);
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
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RecuperaRefreshTokenAsync(DadosUsuarioToken userToken)
        {
            string? token = userToken.Token ?? throw new ArgumentException(nameof(userToken));
            string? refreshToken = userToken.RefreshToken ?? throw new ArgumentException(nameof(userToken));
            var principal = _tokenService.CapturaClaimsDoTokenExpirado(token);
            if (principal == null)
            {
                return BadRequest("Token inválido.");
            }

            var novoUsuarioDTO = new AgendadoUser
            {
                Email = principal.Identity.Name,
                PasswordHash = principal.Claims.FirstOrDefault(c => c.Type == "password")?.Value,
            };

            var agendamentoUser = await _userManager.FindByEmailAsync(novoUsuarioDTO.Email!);

            if (agendamentoUser == null || !agendamentoUser.RefreshToken!.Equals(refreshToken) || agendamentoUser.TempoExpiracao < DateTime.UtcNow)
            {
                return BadRequest("Refresh token inválido.");
            }

            var novoToken = await _tokenService.GerarTokenDeUsuarioAsync(novoUsuarioDTO);
            var novoRefreshToken = _tokenService.GerarRefreshToken();

            agendamentoUser.RefreshToken = novoRefreshToken;
            agendamentoUser.TempoExpiracao = DateTime.UtcNow.AddMinutes(double.Parse(_config["JWTTokenConfiguration:RefreshExpireInMinutes"]));

         
            await _userManager.UpdateAsync(agendamentoUser);

            return Ok(new { novoToken.Token, novoRefreshToken });
        }
    }
}

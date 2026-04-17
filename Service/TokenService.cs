namespace Agendado.Service
{
    using Agendado.Application.Dto;
    using Agendado.Domain.Model;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;

    public class TokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<AgendadoUser> _userManager;

        public TokenService(IConfiguration config, UserManager<AgendadoUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        public async Task<DadosUsuarioToken> GerarTokenDeUsuarioAsync(AgendadoUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var chave = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JWT:KEY"]!));

            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var expiracao = DateTime.UtcNow.AddMinutes(
                Convert.ToInt32(_config["JWTTokenConfiguration:ExpireInMinutes"]));

            var token = new JwtSecurityToken(
                issuer: _config["JWT:ISSUER"],
                audience: _config["JWT:AUDIENCE"],
                claims: claims,
                expires: expiracao,
                signingCredentials: credenciais
            );

            return new DadosUsuarioToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracao = expiracao,
                Autenticado = true
            };
        }
        public string GerarRefreshToken()
        {
            var bytes = new byte[128];
            using var numeroRandomico = RandomNumberGenerator.Create();
            numeroRandomico.GetBytes(bytes);
            var refreshToken = Convert.ToBase64String(bytes);
            return refreshToken;
        }

        internal ClaimsPrincipal CapturaClaimsDoTokenExpirado(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("O token não pode ser nulo ou vazio.", nameof(token));
            }
            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:KEY"]!));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["JWT:Issuer"],
                ValidAudience = _config["JWT:Audience"],
                IssuerSigningKey = chave
            };

            var tokenHandlerValidator = new JwtSecurityTokenHandler();
            var principal = tokenHandlerValidator.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("O token é inválido ou não utiliza o algoritmo esperado.");
            }
            return principal;
        }
    }
}

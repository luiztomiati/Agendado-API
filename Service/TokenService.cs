namespace Agendado.Service
{
    using Agendado.Dto;
    using Agendado.Model;
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

        public async Task<DadosUsuarioToken> GerarTokenDeUsuario(AgendadoUser user)
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
    }
}

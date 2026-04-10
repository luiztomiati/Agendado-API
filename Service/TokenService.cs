namespace Agendado.Service
{
    using Agendado.Dto;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public DadosUsuarioToken GerarTokenDeUsuario(DadosLogin usuario)
        {
            try
            {
                var claims = new[]
                {
            new Claim("TommyLtda", "C#"),
            new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
                var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTKey:key"]!));

                var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

                var expiracao = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["JWTTokenConfiguration:ExpireInMinutes"]));

                JwtSecurityToken token = null;

                try
                {
                    token = new JwtSecurityToken(
                        issuer: _config["JWTTokenConfiguration:Issuer"],
                        audience: _config["JWTTokenConfiguration:Audience"],
                        claims: claims,
                        expires: expiracao,
                        signingCredentials: credenciais
                    );
                }
                catch (Exception exc)
                {
                    throw new ArgumentException("Encontrado erro ao gerar Token!", exc);
                }

                return new DadosUsuarioToken()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiracao = expiracao,
                    Autenticado = true
                };
            }
            catch (Exception exc)
            {
                throw new ArgumentException("Erro ao gerar token", exc);
            }
        }
    }
}

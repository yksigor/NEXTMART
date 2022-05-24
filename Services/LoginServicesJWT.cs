using Domain.Models;
using Domain.Security;
using Microsoft.IdentityModel.Tokens;
using Services.Contratos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Services
{
    public class LoginServicesJWT : ILoginServicesJWT
    {
        private readonly TokenConfigurations tokenConfigurations;
        private readonly SigningConfigurations signingConfigurations;

        public LoginServicesJWT(TokenConfigurations tokenConfigurations, SigningConfigurations signingConfigurations)
        {
            this.tokenConfigurations = tokenConfigurations;
            this.signingConfigurations = signingConfigurations;
        }

        public Token GerarToken(Usuario request)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(request.Username, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, request.Username),
                        new Claim(ClaimTypes.NameIdentifier, request.Id.ToString())
                    }
                );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromSeconds(tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenConfigurations.Issuer,
                Audience = tokenConfigurations.Audience,
                SigningCredentials = signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            var token = handler.WriteToken(securityToken);

            var JWT = new Token(true, dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"), token, "OK");

            return JWT;
        }
    }
}

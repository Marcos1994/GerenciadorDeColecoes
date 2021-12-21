using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MinhasColecoes.API.Interfaces;
using MinhasColecoes.Aplicacao.Models.View;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.API.Services
{
	public class JWTService : IJWTService
	{
		private readonly IConfiguration configuration;
		public JWTService(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public string GerarToken(UsuarioLoginViewModel usuario)
		{
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			byte[] chave = Encoding.ASCII.GetBytes(configuration.GetSection("JWT:Secret").Value);

			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, usuario.Nome)
			};

			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration.GetSection("JWT:ExpiraEmMinutos").Value)),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chave), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}

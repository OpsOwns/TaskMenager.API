using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TaskMenagerService.Extensions.Configuration;
using TaskMenagerService.Interfaces;
namespace TaskMenagerService.Security
{
	public class TokenGenerator : ITokenGenerator
	{
		public string GenerateToken(IConfiguration configuration)
		{
			var symetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSecurityKey()));
			var creditinals = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256Signature);
			var token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(configuration.GetValidIssuer(), configuration.GetValidAudience(), expires: DateTime.Now.AddYears(10), signingCredentials: creditinals));
			return token;
		}
	}
}

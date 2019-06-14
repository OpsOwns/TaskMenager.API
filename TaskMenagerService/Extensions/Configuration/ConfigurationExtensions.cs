using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace TaskMenagerService.Extensions.Configuration
{
	public static class ConfigurationExtensions
	{
		public static string GetSecurityKey(this IConfiguration configuration) => configuration.GetValue<string>("Authentication:SecurityKey");
		public static string GetValidIssuer(this IConfiguration configuration) => configuration.GetValue<string>("Authentication:Issuer");
		public static string GetValidAudience(this IConfiguration configuration) => configuration.GetValue<string>("Authentication:Audience");
		public static SymmetricSecurityKey GetSymmetricSecurityKey(this IConfiguration configuration)
		{
			var issuerSigningKey = configuration.GetSecurityKey();
			var data = Encoding.UTF8.GetBytes(issuerSigningKey);
			var result = new SymmetricSecurityKey(data);
			return result;
		}
	}
}

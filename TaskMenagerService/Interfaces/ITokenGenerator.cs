using Microsoft.Extensions.Configuration;

namespace TaskMenagerService.Interfaces
{
	public interface ITokenGenerator
	{
		string GenerateToken(IConfiguration configuration);
	}
}
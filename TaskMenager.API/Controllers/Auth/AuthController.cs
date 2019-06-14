using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TaskMenagerService.Interfaces;

namespace TaskMenager.API.Controllers.Auth
{
	[Route("[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		ITokenGenerator _tokenGenerator;
		readonly IConfiguration _configuration;
		public AuthController(ITokenGenerator tokenGenerator, IConfiguration configuration)
		{
			_tokenGenerator = tokenGenerator;
			_configuration = configuration;
		}
		[HttpPost("Token")]
		public ActionResult GetToken() => Ok(new { Token = _tokenGenerator.GenerateToken(_configuration) });
	}
}
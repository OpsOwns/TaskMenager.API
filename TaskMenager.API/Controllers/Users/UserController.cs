using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskMenager.API.Models;
using TaskMenagerService.Interfaces;
using TaskMenagerService.Models;
using TaskMenagerService.Security;
using TaskMenagerService.Services.UserService.Commands.UserDeleteCommand;
using TaskMenagerService.Services.UserService.Commands.UserUpdateCommand;
using TaskMenagerService.Services.UserService.Queries;

namespace TaskMenager.API.Controllers.Users
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class UserController : ControllerBase
	{
		private readonly IMediator _mediatr;
		private readonly IAesAlgorithm _aesAlgorithm;
		private readonly IValidator<UserSaveCommand> _validator;
		private UserResponse userResponse;
		public UserController(IMediator mediatr, IAesAlgorithm aesAlgorithm, IValidator<UserSaveCommand> validator)
		{
			_mediatr = mediatr;
			_aesAlgorithm = aesAlgorithm;
			_validator = validator;
			userResponse = new UserResponse();
		}

		[HttpGet]
		[Route("delete/{user_Id}")]
		public async Task<IActionResult> Delete(int user_Id)
		{
			await _mediatr.Send(new UserDeleteCommand { User_Id = user_Id });
			return Ok(userResponse.Message = "Skasowano");
		}
		[HttpGet]
		[Route("Login/{login}/{password}")]
		public async Task<IActionResult> UserLogin(string login, string password)
		{
			var list = await _mediatr.Send(new UserLoginQuery().CreateQuery(login, password));
			var data = JsonConvert.SerializeObject(list);
			var result = _aesAlgorithm.Encrypt(data);
			userResponse.Result = result;
			userResponse.Message = "Wynik logowania";
			return Ok(userResponse);
		}
		[HttpGet]
		[Route("UserList/{login}")]
		public async Task<IActionResult> UserList(string login)
		{
			var list = await _mediatr.Send(new UserListQuery().CreateList(login));
			var data = JsonConvert.SerializeObject(list);
			var result = _aesAlgorithm.Encrypt(data);
			userResponse.Result = result;
			userResponse.Message = "Lista";
			return Ok(userResponse);
		}

		[HttpPost]
		[Route("Update")]
		public async Task<IActionResult> Update([FromBody] string userUpdate)
		{
			var jsonData = _aesAlgorithm.Decrypt(userUpdate);
			var user = JsonConvert.DeserializeObject<UserUpdateCommand>(jsonData);
			await _mediatr.Send(user);
			return Ok(userResponse.Message = "Poprawiono");
		}

		[HttpPost]
		[Route("Save")]
		public async Task<IActionResult> Save([FromBody] string userRegister)
		{
			var jsonData = _aesAlgorithm.Decrypt(userRegister);
			var user = JsonConvert.DeserializeObject<UserSaveCommand>(jsonData);
			var validationResult = _validator.Validate(user);
			if (validationResult.IsValid)
			{
				await _mediatr.Send(user);
				return Ok(userResponse.Message = "Dodano");
			}
			else
				return BadRequest(userResponse.Message = "Błąd, dane nie poprawne");
		}
		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] string loginData)
		{
			var jsonData = _aesAlgorithm.Decrypt(loginData);
			var user = JsonConvert.DeserializeObject<UserLoginQuery>(jsonData);
			var result = await _mediatr.Send(user);
			var jsnoEncrypt = _aesAlgorithm.Encrypt(JsonConvert.SerializeObject(result));
			userResponse.Result = jsnoEncrypt;
			userResponse.Message = "Zalogowano";
			if (result != null)
				return Ok(userResponse);
			else
			{
				userResponse.Message = "Błędne dane";
				return BadRequest(userResponse);
			}
		}
	}
}
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskMenager.API.Models;
using TaskMenagerService.Interfaces;
using TaskMenagerService.Services.CommentService.Commands.CommentSaveCommand;
using TaskMenagerService.Services.CommentService.Commands.CommentUpdateCommand;

namespace TaskMenager.API.Controllers.Comments
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class CommentController : ControllerBase
	{
		private readonly IMediator _mediatr;
		private readonly IAesAlgorithm _aesAlgorithm;
		private readonly IValidator<CommentSaveCommand> _validator;
		private UserResponse userResponse;
		public CommentController(IMediator mediatr, IAesAlgorithm aesAlgorithm, IValidator<CommentSaveCommand> validator)
		{
			_mediatr = mediatr;
			_aesAlgorithm = aesAlgorithm;
			_validator = validator;
			userResponse = new UserResponse();
		}

		[HttpPut]
		[Route("Update")]
		public async Task<IActionResult> Update([FromBody] string commentUpdate)
		{
			var jsonData = _aesAlgorithm.Decrypt(commentUpdate);
			var comment = JsonConvert.DeserializeObject<CommentUpdateCommand>(jsonData);
			await _mediatr.Send(comment);
			return Ok(userResponse.Message = "Poprawiono");
		}
		[HttpPost]
		[Route("Save")]
		public async Task<IActionResult> Save([FromBody] string CommentRegister)
		{
			var jsonData = _aesAlgorithm.Decrypt(CommentRegister);
			var comment = JsonConvert.DeserializeObject<CommentSaveCommand>(jsonData);
			var validationResult = _validator.Validate(comment);
			if (validationResult.IsValid)
			{
				await _mediatr.Send(comment);
				return Ok(userResponse.Message = "Dodano");
			}
			else
			{
				var result = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
				userResponse.Result = result.Aggregate((data, value) => $"{data}, {value}");
				userResponse.Message = "Błąd, dane nie poprawne";
				return BadRequest(userResponse);
			}
		}

	}
}
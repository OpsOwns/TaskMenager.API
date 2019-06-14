using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskMenager.API.Models;
using TaskMenagerService.Interfaces;
using TaskMenagerService.Services.TaskService.Commands.TaskDeleteCommand;
using TaskMenagerService.Services.TaskService.Commands.TaskSaveCommand;
using TaskMenagerService.Services.TaskService.Commands.TaskUpdateCommand;
using TaskMenagerService.Services.TaskService.Queries;
namespace TaskMenager.API.Controllers.Tasks
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class TaskController : ControllerBase
	{
		private readonly IMediator _mediatr;
		private readonly IAesAlgorithm _aesAlgorithm;
		private readonly IValidator<TaskSaveCommand> _validator;
		private UserResponse userResponse;
		public TaskController(IMediator mediatr, IAesAlgorithm aesAlgorithm, IValidator<TaskSaveCommand> validator)
		{
			_mediatr = mediatr;
			_aesAlgorithm = aesAlgorithm;
			_validator = validator;
			userResponse = new UserResponse();
		}
		[HttpGet]
		[Route("TaskDoneList")]
		public async Task<IActionResult> TaskDoneList()
		{
			var list = await _mediatr.Send(new TasksDoneListQuery());
			var data = JsonConvert.SerializeObject(list);
			var result = _aesAlgorithm.Encrypt(data);
			userResponse.Result = result;
			userResponse.Message = "Lista";
			return Ok(userResponse);
		}
		[HttpGet]
		[Route("TaskList/{user_Id}")]
		public async Task<IActionResult> TaskList(int user_Id)
		{
			var list = await _mediatr.Send(new TasksListQuery() { User_Id = user_Id });
			var data = JsonConvert.SerializeObject(list);
			var result = _aesAlgorithm.Encrypt(data);
			userResponse.Result = result;
			userResponse.Message = "Lista";
			return Ok(userResponse);
		}
		[HttpGet]
		[Route("delete/{task_Id}")]
		public async Task<IActionResult> Delete(int task_Id)
		{
			await _mediatr.Send(new TaskDeleteCommand { Task_id = task_Id });
			return Ok(userResponse.Message = "Skasowano");
		}
		[HttpPost]
		[Route("Update")]
		public async Task<IActionResult> Update([FromBody] string taskUpdate)
		{

			var jsonData = _aesAlgorithm.Decrypt(taskUpdate);
			var task = JsonConvert.DeserializeObject<TaskUpdateCommand>(jsonData);
			await _mediatr.Send(task);
			return Ok(userResponse.Message = "Poprawiono");

		}
		[HttpPost]
		[Route("Save")]
		public async Task<IActionResult> Save([FromBody] string taskRegister)
		{
			var jsonData = _aesAlgorithm.Decrypt(taskRegister);
			var task = JsonConvert.DeserializeObject<TaskSaveCommand>(jsonData);
			var validationResult = _validator.Validate(task);
			if (validationResult.IsValid)
			{
				await _mediatr.Send(task);
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
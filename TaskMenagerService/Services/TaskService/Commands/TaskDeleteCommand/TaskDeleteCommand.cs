using MediatR;
namespace TaskMenagerService.Services.TaskService.Commands.TaskDeleteCommand
{
	public class TaskDeleteCommand : IRequest
	{
		public int Task_id { get; set; }
	}
}

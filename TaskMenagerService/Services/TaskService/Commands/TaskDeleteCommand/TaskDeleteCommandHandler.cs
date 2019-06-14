using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TaskMenagerService.Exceptions;
using TaskMenagerService.Interfaces;

namespace TaskMenagerService.Services.TaskService.Commands.TaskDeleteCommand
{
	public class TaskDeleteCommandHandler : AsyncRequestHandler<TaskDeleteCommand>, ITaskDeleteCommandHandler
	{
		private readonly ITasksDbContext _context;
		private readonly ILogger<TaskDeleteCommand> _logger;
		public TaskDeleteCommandHandler(ITasksDbContext context, ILogger<TaskDeleteCommand> logger)
		{
			_context = context;
			_logger = logger;
		}
		protected override async Task Handle(TaskDeleteCommand taskDTO, CancellationToken cancellationToken)
		{
			try
			{
				var task = await _context.Tasks.FirstOrDefaultAsync(log => log.Task_Id == taskDTO.Task_id);
				if (task != null)
				{
					task.Done = true;
					await _context.SaveChangesAsync();
					_logger.LogInformation("Delete value: {@UserDTO}", taskDTO);
				}
				else
					_logger.LogInformation($"Delete fail, Task {task.CurrentTask} not found value: {taskDTO}");
			}
			catch (TasksException ex)
			{
				_logger.LogError($"Error {new TasksException("Delete", taskDTO, ex.Message)}");
				throw new TasksException("Delete", taskDTO, ex.Message);
			}
		}
	}
}

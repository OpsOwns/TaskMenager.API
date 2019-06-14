using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TaskMenagerService.Exceptions;
using TaskMenagerService.Interfaces;
namespace TaskMenagerService.Services.TaskService.Commands.TaskUpdateCommand
{
	public class TaskUpdateCommandHandler : AsyncRequestHandler<TaskUpdateCommand>, ITaskUpdateCommandHandler
	{
		private readonly ITasksDbContext _context;
		private readonly ILogger<TaskUpdateCommand> _logger;
		private readonly IMapper _mapper;
		public TaskUpdateCommandHandler(ITasksDbContext context, ILogger<TaskUpdateCommand> logger, IMapper mapper)
		{
			_context = context;
			_logger = logger;
			_mapper = mapper;
		}

		protected override async Task Handle(TaskUpdateCommand taskDTO, CancellationToken cancellationToken)
		{
			try
			{
				var task = await _context.Tasks.FirstOrDefaultAsync(log => log.Task_Id == taskDTO.Task_Id);
				if (task != null)
				{
					if (!string.IsNullOrEmpty(taskDTO.CurrentTask))
						task.CurrentTask = taskDTO.CurrentTask;
					if(!string.IsNullOrEmpty(taskDTO.UserDescription))
					task.UserDescription = taskDTO.UserDescription;
					await _context.SaveChangesAsync();
					_logger.LogInformation("Update value: {@UserDTO}", taskDTO);
				}
				else
					_logger.LogInformation($"Update fail, Task {taskDTO.CurrentTask} not found value: {taskDTO}");
			}
			catch (TasksException ex)
			{
				_logger.LogError($"Error {new TasksException("Update", taskDTO, ex.Message)}");
				throw new TasksException("Update", taskDTO, ex.Message);
			}
		}
	}
}

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TaskMenagerService.Exceptions;
using TaskMenagerService.Interfaces;
using TaskMenagerService.Models.TemplateTable;

namespace TaskMenagerService.Services.TaskService.Commands.TaskSaveCommand
{
	public class TaskSaveCommandHandler : AsyncRequestHandler<TaskSaveCommand>, ITaskSaveCommandHandler
	{
		private readonly ITasksDbContext _context;
		private readonly ILogger<TaskSaveCommand> _logger;
		private readonly IMapper _mapper;
		public TaskSaveCommandHandler(ITasksDbContext context, ILogger<TaskSaveCommand> logger, IMapper mapper)
		{
			_context = context;
			_logger = logger;
			_mapper = mapper;
		}
		protected override async Task Handle(TaskSaveCommand taskDTO, CancellationToken cancellationToken)
		{
			try
			{
				await _context.Tasks.AddAsync(_mapper.Map<Tasks>(taskDTO));
				await _context.SaveChangesAsync();
				_logger.LogInformation("Insert value: {@TaskDTO}", taskDTO);
			}
			catch (TasksException ex)
			{
				_logger.LogError($"Error {new TasksException("Insert", taskDTO, ex.Message)}");
				throw new TasksException("Insert", taskDTO, ex.Message);
			}
		}
	}
}

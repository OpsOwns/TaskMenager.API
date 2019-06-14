using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskMenagerService.Exceptions;
using TaskMenagerService.Interfaces;

namespace TaskMenagerService.Services.TaskService.Queries
{
	public class TaskListQueryHandler : IRequestHandler<TasksListQuery, IEnumerable<TasksListQuery>>, ITaskListQueryHandler
	{
		private readonly ITasksDbContext _context;
		private readonly ILogger<TasksListQuery> _logger;
		private readonly IMapper _mapper;
		public TaskListQueryHandler(ITasksDbContext context, ILogger<TasksListQuery> logger, IMapper mapper)
		{
			_context = context;
			_logger = logger;
			_mapper = mapper;
		}
		public async Task<IEnumerable<TasksListQuery>> Handle(TasksListQuery taskDTO, CancellationToken cancellationToken)
		{
			try
			{
				var sqlQuerry = await _context.TasksComments.FromSql(@"select ta.Task_Id, ta.CurrentTask, ta.DateCreate, ta.UserDescription, ta.DateEnd, ta.Done, us.Login, com.Comment from Tasks ta
													   Left join Users us ON us.User_Id = ta.FK_User_Id left join Comments com ON com.FK_Task_Id = ta.Task_Id WHERE ta.FK_User_Id = {0} and ta.Done = 0", taskDTO.User_Id).ToListAsync();
				var listOfTasks = _mapper.Map<List<TasksListQuery>>(sqlQuerry);
				_logger.LogInformation($"User {taskDTO.User_Id} get list");
				return listOfTasks;
			}
			catch (TasksException ex)
			{
				_logger.LogError($"Error {new TasksException("List", null, ex.Message)}");
				throw new TasksException("List", null, ex.Message);
			}
		}
	}
}

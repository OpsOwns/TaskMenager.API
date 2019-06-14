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
	public class TasksDoneListQueryHandler : IRequestHandler<TasksDoneListQuery, IEnumerable<TasksDoneListQuery>>, ITasksDoneListQueryHandler
	{
		private readonly ITasksDbContext _context;
		private readonly ILogger<TasksDoneListQuery> _logger;
		private readonly IMapper _mapper;
		public TasksDoneListQueryHandler(ITasksDbContext context, ILogger<TasksDoneListQuery> logger, IMapper mapper)
		{
			_context = context;
			_logger = logger;
			_mapper = mapper;
		}
		public async Task<IEnumerable<TasksDoneListQuery>> Handle(TasksDoneListQuery taskDTO, CancellationToken cancellationToken)
		{
			try
			{
				var tasksList = await _context.TasksComments.FromSql(@"select ta.Task_Id, ta.CurrentTask, ta.DateCreate, ta.UserDescription, ta.DateEnd, ta.Done, us.Login, com.Comment from Tasks ta
													   Left join Users us ON us.User_Id = ta.FK_User_Id left join Comments com ON com.FK_Task_Id = ta.Task_Id WHERE ta.Done <> 0").ToListAsync();	
				var listOfTasks = _mapper.Map<IEnumerable<TasksDoneListQuery>>(tasksList);
				_logger.LogInformation($"User {taskDTO.Login} get list");
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

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskMenagerService.Exceptions;
using TaskMenagerService.Interfaces;

namespace TaskMenagerService.Services.NotifyService.Queries
{
	public class NotifyQueryHandler : IRequestHandler<NotifyQuery, List<NotifyQuery>>, INotifyQueryHandler
	{
		private readonly ITasksDbContext _context;
		private readonly ILogger<NotifyQuery> _logger;
		private readonly IMapper _mapper;
		public NotifyQueryHandler(ITasksDbContext context, ILogger<NotifyQuery> logger, IMapper mapper)
		{
			_context = context;
			_logger = logger;
			_mapper = mapper;
		}
		public async Task<List<NotifyQuery>> Handle(NotifyQuery notifyDTO, CancellationToken cancellationToken)
		{
			try
			{
				var notifiesList = await _context.TasksNotify.FromSql(@"Select ts.CurrentTask, cm.Comment, us.Login, ts.DateCreate, ts.DateEnd from Tasks ts
																	Left join Users us ON us.User_Id = ts.FK_User_Id left join
																	Comments cm ON cm.FK_Task_Id = ts.Task_Id Where Done <> 0 And us.Flag <> 'A'").ToListAsync();
				var listOfEndTasks = _mapper.Map<List<NotifyQuery>>(notifiesList);
				_logger.LogInformation($"User {notifyDTO.Login} get list");
				return listOfEndTasks;
			}
			catch (TasksException ex)
			{
				_logger.LogError($"Error {new TasksException("List", null, ex.Message)}");
				throw new TasksException("List", null, ex.Message);
			}
		}
	}
}

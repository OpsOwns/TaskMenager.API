using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskMenagerService.Exceptions;
using TaskMenagerService.Interfaces;
using TaskMenagerService.Services.UserService.Queires;

namespace TaskMenagerService.Services.UserService.Queries
{
	public class UserListQueryHandler : IRequestHandler<UserListQuery, IEnumerable<UserListQuery>>, IUserListQueryHandler
	{
		private readonly ITasksDbContext _context;
		private readonly ILogger<UserLoginQuery> _logger;
		private readonly IMapper _mapper;
		private readonly IMediator _mediator;
		public UserListQueryHandler(ITasksDbContext context, ILogger<UserLoginQuery> logger, IMapper mapper)
		{
			_context = context;
			_logger = logger;
			_mapper = mapper;
		}
		public async Task<IEnumerable<UserListQuery>> Handle(UserListQuery userDTO, CancellationToken cancellationToken)
		{
			try
			{
				var users = await _context.Users.Where(log => log.LockAccount == false).ToListAsync();
				var usersDTO = _mapper.Map<IEnumerable<UserListQuery>>(users);
				_logger.LogInformation($"User {userDTO.Login} get list");
				return usersDTO;
			}
			catch (UserException ex)
			{
				_logger.LogError($"Error {new UserException("List", null, ex.Message)}");
				throw new UserException("List", null, ex.Message);
			}
		}
	}
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TaskMenagerService.Exceptions;
using TaskMenagerService.Interfaces;

namespace TaskMenagerService.Services.UserService.Commands.UserDeleteCommand
{
	public class UserDeleteCommandHandler : AsyncRequestHandler<UserDeleteCommand>, IUserDeleteCommandHandler
	{
		private readonly ITasksDbContext _context;
		private readonly ILogger<UserDeleteCommand> _logger;
		public UserDeleteCommandHandler(ITasksDbContext context, ILogger<UserDeleteCommand> logger)
		{
			_context = context;
			_logger = logger;
		}
		protected override async Task Handle(UserDeleteCommand userDTO, CancellationToken cancellationToken)
		{
			try
			{
				var user = await _context.Users.FirstOrDefaultAsync(log => log.User_Id == userDTO.User_Id);
				if (user != null)
				{
					user.LockAccount = true;
					await _context.SaveChangesAsync();
					_logger.LogInformation("Delete value: {@UserDTO}", userDTO);
				}
				else
					_logger.LogInformation($"Delete fail, login {user.Login} not found value: {userDTO}");
			}
			catch (UserException ex)
			{
				_logger.LogError($"Error {new UserException("Delete", userDTO, ex.Message)}");
				throw new UserException("Delete", userDTO, ex.Message);
			}
		}
	}
}

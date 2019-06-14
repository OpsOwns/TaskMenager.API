using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TaskMenagerService.Exceptions;
using TaskMenagerService.Interfaces;
using TaskMenagerService.Models.TemplateTable;
using TaskMenagerService.Security;

namespace TaskMenagerService.Services.UserService.Queries
{
	public class UserLoginQueryHandler : IRequestHandler<UserLoginQuery, Users>, IUserLoginQueryHandler
	{
		private readonly IAesAlgorithm _aesAlgorithm;
		private readonly ITasksDbContext _context;
		private readonly ILogger<UserLoginQuery> _logger;
		public UserLoginQueryHandler(ITasksDbContext context, ILogger<UserLoginQuery> logger, IAesAlgorithm aesAlgorithm)
		{
			_context = context;
			_logger = logger;
			_aesAlgorithm = aesAlgorithm;
		}
		public async Task<Users> Handle(UserLoginQuery userDTO, CancellationToken cancellationToken)
		{
			try
			{
				var user = await _context.Users.FirstOrDefaultAsync(log => log.Login == userDTO.Login);
				if (user != null)
				{
					if (user.Password.VerifyPassword(userDTO.Password))
					{
						_logger.Log(LogLevel.Information, $"User {userDTO.Login} logged");
						return user;
					}
					else
					{
						_logger.Log(LogLevel.Information, $"User {userDTO.Login} try loggin");
						return null;
					}
				}
				_logger.Log(LogLevel.Information, $"User {userDTO.Login} Login not exists");
				return null;
			}
			catch (UserException ex)
			{
				_logger.LogError($"Error {new UserException("Login", userDTO, ex.Message)}");
				throw new UserException("Login", userDTO, ex.Message);
			}
		}
	}
}

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TaskMenagerService.Exceptions;
using TaskMenagerService.Interfaces;
using TaskMenagerService.Security;

namespace TaskMenagerService.Services.UserService.Commands.UserUpdateCommand
{
	public class UserUpdateCommandHandler : AsyncRequestHandler<UserUpdateCommand>, IUserUpdateCommandHandler
	{
		private readonly ITasksDbContext _context;
		private readonly IMapper _mapper;
		private readonly ILogger<UserUpdateCommandHandler> _logger;
		public UserUpdateCommandHandler(ITasksDbContext context, ILogger<UserUpdateCommandHandler> logger, IMapper mapper)
		{
			_context = context;
			_logger = logger;
			_mapper = mapper;
		}
		protected override async Task Handle(UserUpdateCommand userDTO, CancellationToken cancellationToken)
		{
			try
			{
				var user = await _context.Users.FirstOrDefaultAsync(log => log.User_Id == userDTO.User_Id);
				if (user != null)
				{
					user.Login = userDTO.Login;
					user.Password = userDTO.Password.Encrypt();
					user.Email = userDTO.Email;
					await _context.SaveChangesAsync();
					_logger.LogInformation("Update value: {@UserDTO}", userDTO);
				}
				else
					_logger.LogInformation($"Update fail, login {userDTO.Login} not found value: {userDTO}");
			}
			catch (UserException ex)
			{
				_logger.LogError($"Error {new UserException("Update", userDTO, ex.Message)}");
				throw new UserException("Update", userDTO, ex.Message);
			}
		}
	}
}

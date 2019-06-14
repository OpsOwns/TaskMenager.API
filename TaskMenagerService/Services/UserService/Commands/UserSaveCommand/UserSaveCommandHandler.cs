using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TaskMenagerService.Exceptions;
using TaskMenagerService.Interfaces;
using TaskMenagerService.Models;
using TaskMenagerService.Models.TemplateTable;
using TaskMenagerService.Security;

namespace TaskMenagerService.UserService.Commands
{
	public class UserSaveCommandHandler : AsyncRequestHandler<UserSaveCommand>, IUserSaveCommandHandler
	{
		private readonly ITasksDbContext _context;
		private readonly ILogger<UserSaveCommandHandler> _logger;
		private readonly IMapper _mapper;
		public UserSaveCommandHandler(ITasksDbContext context, ILogger<UserSaveCommandHandler> logger, IMapper mapper)
		{
			_context = context;
			_logger = logger;
			_mapper = mapper;
		}
		protected override async Task Handle(UserSaveCommand userDTO, CancellationToken cancellationToken)
		{
			try
			{
				userDTO.Password = userDTO.Password.Encrypt();
				await _context.Users.AddAsync(_mapper.Map<Users>(userDTO));
				await _context.SaveChangesAsync();
				_logger.LogInformation("Insert value: {@UserDTO}", userDTO);
			}
			catch (System.Exception ex)
			{
				_logger.LogError($"Error {new UserException("Insert", userDTO, ex.Message)}");
				throw new UserException("Insert", userDTO, ex.Message);
			}
		}
	}
}

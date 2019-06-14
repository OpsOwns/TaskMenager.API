using MediatR;
namespace TaskMenagerService.Services.UserService.Commands.UserDeleteCommand
{
	public class UserDeleteCommand : IRequest
	{
		public int User_Id { get; set; }
	}
}

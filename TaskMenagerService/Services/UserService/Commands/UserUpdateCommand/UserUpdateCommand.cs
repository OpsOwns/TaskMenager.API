using MediatR;
using Newtonsoft.Json;
using TaskMenagerService.Models.Abstract;

namespace TaskMenagerService.Services.UserService.Commands.UserUpdateCommand
{
	public class UserUpdateCommand : UserAbstract<UserUpdateCommand>, IRequest
	{
		[JsonProperty]
		public int User_Id { get; set; }
		[JsonProperty]
		public string Login { get; private set; }
		[JsonProperty]
		public string Password { get; private set; }
		[JsonProperty]
		public string Email { get; private set; }
		[JsonProperty]
		public string Flag { get; private set; }
		[JsonProperty]
		public bool LockAccount { get; private set; }
		public override UserUpdateCommand CreateUser(string login, string password, string email, string flag)
		{
			Login = login;
			Password = password;
			Email = email;
			Flag = flag;
			return this;
		}
	}
}

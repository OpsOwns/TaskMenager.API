using MediatR;
using Newtonsoft.Json;
using TaskMenagerService.Models.TemplateTable;

namespace TaskMenagerService.Services.UserService.Queries
{
	public class UserLoginQuery : IRequest<Users>
	{
		[JsonProperty]
		public string Login { get; private set; }
		[JsonProperty]
		public string Password { get; private set; }
		public UserLoginQuery CreateQuery(string login, string password)
		{
			Login = login;
			Password = password;
			return this;
		}
	}
}

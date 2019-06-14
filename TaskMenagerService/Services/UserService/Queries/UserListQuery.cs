using MediatR;
using System.Collections.Generic;
namespace TaskMenagerService.Services.UserService.Queries
{
	public class UserListQuery : IRequest<IEnumerable<UserListQuery>>
	{
		public int User_Id { get; private set; }
		public string Login { get; private set; }
		public string Email { get; private set; }

		public UserListQuery CreateList(string login, string email = "", int user_id = 0)
		{
			Login = login;
			Email = email;
			User_Id = user_id;
			return this;
		}
	}
}

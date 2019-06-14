using MediatR;
using Newtonsoft.Json;
using System;
using TaskMenagerService.Models.Abstract;

namespace TaskMenagerService.Models
{
	public class UserSaveCommand : UserAbstract<UserSaveCommand>, IRequest
	{
		public int User_Id { get; private set; }
		[JsonProperty]
		public string Login { get; private set; }
		[JsonProperty]
		public string Password { get; set; }
		[JsonProperty]
		public string Email { get; private set; }
		[JsonProperty]
		public string Flag { get; private set; }
		[JsonProperty]
		public DateTime DateCreate { get; private set; }
		[JsonProperty]
		public bool LockAccount { get; private set; }

		public override UserSaveCommand CreateUser(string login, string password, string email, string flag)
		{
			Login = login;
			Password = password;
			Email = email;
			Flag = flag;
			LockAccount = false;
			DateCreate = DateTime.UtcNow;
			return this;
		}
	}
}

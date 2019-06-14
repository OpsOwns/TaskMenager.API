using MediatR;
using System;
using System.Collections.Generic;
namespace TaskMenagerService.Services.NotifyService.Queries
{
	public class NotifyQuery : IRequest<List<NotifyQuery>>
	{
		public string CurrentTask { get; private set; }
		public string Comment { get; private set; }
		public string Login { get; set; }
		public DateTime DateEnd { get; private set; }
		public DateTime DateCreate { get; private set; }

		public NotifyQuery CreateNotify(string currentTask, string comment, string login, DateTime dateCreate, DateTime dateEnd)
		{
			CurrentTask = currentTask;
			Comment = comment;
			Login = login;
			DateCreate = dateCreate;
			DateEnd = dateEnd;
			return this;
		}
	}
}

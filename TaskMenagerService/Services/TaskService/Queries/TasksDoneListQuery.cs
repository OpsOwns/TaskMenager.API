using MediatR;
using System;
using System.Collections.Generic;
namespace TaskMenagerService.Services.TaskService.Queries
{
	public class TasksDoneListQuery : IRequest<IEnumerable<TasksDoneListQuery>>
	{
		public int Task_Id { get; private set; }
		public string CurrentTask { get; private set; }
		public string UserDescription { get; private set; }
		public string Login { get; private set; }
		public bool Done { get; private set; }
		public string Comment { get; private set; }
		public DateTime DateCreate { get; private set; }
		public DateTime DateEnd { get; private set; }
		public TasksDoneListQuery CreateList(int task_Id, string currentTask, string userDescription, string login, bool done, DateTime dateCreate, DateTime dateEnd, string comment)
		{
			Task_Id = task_Id;
			CurrentTask = currentTask;
			UserDescription = userDescription;
			Login = login;
			Done = done;
			DateCreate = dateCreate;
			DateEnd = dateEnd;
			Comment = comment;
			return this;
		}
	}
}

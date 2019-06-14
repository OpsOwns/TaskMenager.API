using MediatR;
using System;
using System.Collections.Generic;
namespace TaskMenagerService.Services.TaskService.Queries
{
	public class TasksListQuery : IRequest<IEnumerable<TasksListQuery>>
	{
		public int Task_Id { get; private set; }
		public string CurrentTask { get; private set; }
		public string UserDescription { get; private set; }
		public int User_Id { get; set; }
		public string Comment { get; private set; }
		public DateTime DateCreate { get; private set; }
		public DateTime DateEnd { get; private set; }

		public TasksListQuery CreateList(int task_Id, string currentTask, string userDescription, int user_Id, DateTime dateCreate, DateTime dateEnd, string comment)
		{
			Task_Id = task_Id;
			CurrentTask = currentTask;
			UserDescription = userDescription;
			User_Id = user_Id;
			DateCreate = dateCreate;
			DateEnd = dateEnd;
			Comment = comment;
			return this;
		}
	}
}

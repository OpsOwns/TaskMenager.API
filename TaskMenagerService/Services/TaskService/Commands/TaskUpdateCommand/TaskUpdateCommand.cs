using MediatR;
using Newtonsoft.Json;
using System;
namespace TaskMenagerService.Services.TaskService.Commands.TaskUpdateCommand
{
	public class TaskUpdateCommand : IRequest
	{
		[JsonProperty]
		public int Task_Id { get; private set; }
		[JsonProperty]
		public string CurrentTask { get; private set; }
		[JsonProperty]
		public string UserDescription { get; private set; }
		[JsonProperty]
		public bool Done { get; private set; }
		[JsonProperty]
		public int FK_User_Id { get; private set; }
		[JsonProperty]
		public DateTime DateEnd { get; private set; }
		public TaskUpdateCommand CreateTask(int task_id, string currentTask, string userDescription, bool done, int fk_user_id, DateTime dateCreate, DateTime dateEnd)
		{
			Task_Id = task_id;
			CurrentTask = currentTask;
			UserDescription = userDescription;
			Done = done;
			FK_User_Id = fk_user_id;
			DateEnd = dateEnd;
			return this;
		}
	}
}

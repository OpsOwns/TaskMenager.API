using MediatR;
using Newtonsoft.Json;
using System;
namespace TaskMenagerService.Services.TaskService.Commands.TaskSaveCommand
{
	public class TaskSaveCommand : IRequest
	{
		[JsonProperty]
		public string CurrentTask { get; private set; }
		[JsonProperty]
		public string UserDescription { get; private set; }
		[JsonProperty]
		public bool Done { get; private set; }
		[JsonProperty]
		public int FK_User_Id { get; private set; }
		[JsonProperty]
		public DateTime DateCreate { get; private set; }
		[JsonProperty]
		public DateTime DateEnd { get; private set; }
		public TaskSaveCommand CreateTask(string currentTask, string userDescription, bool done, int fk_user_id, DateTime dateCreate, DateTime dateEnd)
		{
			CurrentTask = currentTask;
			UserDescription = userDescription;
			Done = done;
			FK_User_Id = fk_user_id;
			DateCreate = dateCreate;
			DateEnd = dateEnd;
			return this;
		}
	}
}

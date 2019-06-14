using MediatR;
using Newtonsoft.Json;
using System;
namespace TaskMenagerService.Services.CommentService.Commands.CommentSaveCommand
{
	public class CommentSaveCommand : IRequest
	{
		[JsonProperty]
		public string Comment { get; private set; }
		[JsonProperty]
		public int FK_Users_Id { get; private set; }
		[JsonProperty]
		public int FK_Task_Id { get; private set; }
		[JsonProperty]
		public DateTime DateCreate { get; private set; }
		public CommentSaveCommand CreateComment(string comment, int user_id, int task_id, DateTime dateCreate)
		{
			Comment = comment;
			FK_Task_Id = task_id;
			FK_Users_Id = user_id;
			DateCreate = dateCreate;
			return this;
		}
	}
}

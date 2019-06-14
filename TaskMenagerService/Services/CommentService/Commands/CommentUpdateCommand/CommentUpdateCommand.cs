using MediatR;
using Newtonsoft.Json;

namespace TaskMenagerService.Services.CommentService.Commands.CommentUpdateCommand
{
	public class CommentUpdateCommand : IRequest
	{
		[JsonProperty]
		public int Task_Id { get; private set; }
		[JsonProperty]
		public int Comment_Id { get; private set; }
		[JsonProperty]
		public string Comment { get; private set; }
		public CommentUpdateCommand CreateComment(int task_Id, int comment_Id, string comment)
		{
			Task_Id = task_Id;
			Comment_Id = comment_Id;
			Comment = comment;
			return this;
		}
	}
}

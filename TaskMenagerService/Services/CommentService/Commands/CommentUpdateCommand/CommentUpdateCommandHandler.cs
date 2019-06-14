using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TaskMenagerService.Exceptions;
using TaskMenagerService.Interfaces;

namespace TaskMenagerService.Services.CommentService.Commands.CommentUpdateCommand
{
	public class CommentUpdateCommandHandler : AsyncRequestHandler<CommentUpdateCommand>, ICommentUpdateCommandHandler
	{
		private readonly ITasksDbContext _context;
		private readonly ILogger<CommentUpdateCommand> _logger;
		private readonly IMapper _mapper;
		public CommentUpdateCommandHandler(ITasksDbContext context, ILogger<CommentUpdateCommand> logger, IMapper mapper)
		{
			_context = context;
			_logger = logger;
			_mapper = mapper;
		}
		protected override async Task Handle(CommentUpdateCommand commentDTO, CancellationToken cancellationToken)
		{
			try
			{
				var comment = await _context.Comments.FirstOrDefaultAsync(com => com.Comment_Id == commentDTO.Comment_Id && com.FK_Task_Id == commentDTO.Task_Id);
				if (comment != null)
				{
					_mapper.Map(commentDTO, comment);
					await _context.SaveChangesAsync();
					_logger.LogInformation("Update value: {@UserDTO}", commentDTO);
				}
				else
					_logger.LogInformation($"Update fail, Comment {commentDTO.Comment_Id} not found value: {commentDTO}");
			}
			catch (CommentsException ex)
			{
				_logger.LogError($"Error {new CommentsException("Update", commentDTO, ex.Message)}");
				throw new CommentsException("Update", commentDTO, ex.Message);
			}
		}
	}
}

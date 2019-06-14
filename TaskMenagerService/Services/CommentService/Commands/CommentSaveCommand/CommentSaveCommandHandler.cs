using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TaskMenagerService.Exceptions;
using TaskMenagerService.Interfaces;
using TaskMenagerService.Models.TemplateTable;
namespace TaskMenagerService.Services.CommentService.Commands.CommentSaveCommand
{
	public class CommentSaveCommandHandler : AsyncRequestHandler<CommentSaveCommand>, ICommentSaveCommandHandler
	{
		private readonly ITasksDbContext _context;
		private readonly ILogger<CommentSaveCommand> _logger;
		private readonly IMapper _mapper;
		public CommentSaveCommandHandler(ITasksDbContext context, ILogger<CommentSaveCommand> logger, IMapper mapper)
		{
			_context = context;
			_logger = logger;
			_mapper = mapper;
		}
		protected override async Task Handle(CommentSaveCommand commentDTO, CancellationToken cancellationToken)
		{
			try
			{
				await _context.Comments.AddAsync(_mapper.Map<Comments>(commentDTO));
				await _context.SaveChangesAsync();
				_logger.LogInformation("Insert value: {@commentDTO}", commentDTO);
			}
			catch (CommentsException ex)
			{
				_logger.LogError($"Error {new CommentsException("Insert", commentDTO, ex.Message)}");
				throw new TasksException("Insert", commentDTO, ex.Message);
			}
		}
	}
}

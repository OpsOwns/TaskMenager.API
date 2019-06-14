using AutoMapper;
using TaskMenagerService.Services.CommentService.Commands.CommentSaveCommand;
using TaskMenagerService.Services.CommentService.Commands.CommentUpdateCommand;
namespace TaskMenagerService.AutoMapper.Comments
{
	public class CommentsDomain : Profile
	{
		public CommentsDomain()
		{
			CreateMap<CommentSaveCommand, Models.TemplateTable.Comments>();
			CreateMap<CommentUpdateCommand, Models.TemplateTable.Comments>();
		}
	}
}

using AutoMapper;
using TaskMenagerService.Services.NotifyService.Queries;
using TaskMenagerService.Services.TaskService.Queries;

namespace TaskMenagerService.AutoMapper.Queries
{
	public class QueriesDomain : Profile
	{
		public QueriesDomain()
		{
			CreateMap<TasksDoneListQuery, Models.TemplateTable.TasksComments>();
			CreateMap<Models.TemplateTable.TasksComments, TasksListQuery>();
			CreateMap<TasksListQuery, Models.TemplateTable.TasksComments>();
			CreateMap<NotifyQuery, Models.TemplateTable.TasksNotify>();
		}
	}
}

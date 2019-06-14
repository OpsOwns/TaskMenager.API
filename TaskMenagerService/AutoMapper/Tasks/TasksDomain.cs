using AutoMapper;
using TaskMenagerService.Services.TaskService.Commands.TaskSaveCommand;
using TaskMenagerService.Services.TaskService.Commands.TaskUpdateCommand;

namespace TaskMenagerService.AutoMapper.Tasks
{
	public class TasksDomain : Profile
	{
		public TasksDomain()
		{
			CreateMap<TaskSaveCommand, Models.TemplateTable.Tasks>();
			CreateMap<TaskUpdateCommand, Models.TemplateTable.Tasks>();
		}
	}
}

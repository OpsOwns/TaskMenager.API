using AutoMapper;
using TaskMenagerService.Models;
using TaskMenagerService.Services.UserService.Commands.UserUpdateCommand;
using TaskMenagerService.Services.UserService.Queries;

namespace TaskMenagerService.AutoMapper.Users
{
	public class UsersDomain : Profile
	{
		public UsersDomain()
		{
			CreateMap<UserSaveCommand, Models.TemplateTable.Users>();
			CreateMap<Models.TemplateTable.Users, UserUpdateCommand>().ReverseMap().ForMember(u => u.User_Id, opt => opt.Ignore());
			CreateMap<Models.TemplateTable.Users, UserListQuery>().ReverseMap();
		}
	}
}

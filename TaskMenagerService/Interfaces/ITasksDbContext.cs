using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskMenagerService.Models.TemplateTable;

namespace TaskMenagerService.Interfaces
{
	public interface ITasksDbContext
	{
		DbSet<Comments> Comments { get; set; }
		DbSet<Tasks> Tasks { get; set; }
		DbSet<Users> Users { get; set; }
		DbSet<TasksNotify> TasksNotify { get; set; }
		DbSet<TasksComments> TasksComments { get; set; }
		Task SaveChangesAsync();
	}
}
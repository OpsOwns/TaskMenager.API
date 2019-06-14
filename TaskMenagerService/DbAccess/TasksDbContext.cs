using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskMenagerService.Interfaces;
using TaskMenagerService.Models.TemplateTable;
namespace TaskMenagerService.DbAccess
{
	public class TasksDbContext : DbContext, ITasksDbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-7GLL7LJ\OPSDB;Initial Catalog=TasksERP;Integrated Security=True");
		}
		public async Task SaveChangesAsync() => await base.SaveChangesAsync();
		#region DbSets
		public DbSet<Tasks> Tasks { get; set; }
		public DbSet<Comments> Comments { get; set; }
		public DbSet<Users> Users { get; set; }
		public DbSet<TasksComments> TasksComments { get; set; }
		public DbSet<TasksNotify> TasksNotify { get; set; }
		#endregion
	}
}

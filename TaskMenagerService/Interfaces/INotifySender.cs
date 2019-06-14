using System.Threading.Tasks;
using TaskMenagerService.Models.TemplateTable;
using TaskMenagerService.Services.NotifyService.Queries;
namespace TaskMenagerService.Interfaces
{
	public interface INotifySender
	{
		Task SendNotify(UserCredentials user, NotifyQuery notify);
	}
}
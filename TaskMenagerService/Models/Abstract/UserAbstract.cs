namespace TaskMenagerService.Models.Abstract
{
	public abstract class UserAbstract<T> where T : class, new()
	{
		public abstract T CreateUser(string login, string password, string email, string flag);
	}
}

using System;
namespace TaskMenagerService.Exceptions
{
	public class TasksException : Exception
	{
		public TasksException(string name, object value, string message) : base($"Exception, operation type:{name} object: {value} has error {message}")
		{
		}
	}
}

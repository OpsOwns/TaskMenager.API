using System;
namespace TaskMenagerService.Exceptions
{
	public class UserException : Exception
	{
		public UserException(string name, object value, string message) :base($"Exception, operation type:{name} object: {value} has error {message}")
		{
		}
	}
}

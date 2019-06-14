using System;
namespace TaskMenagerService.Exceptions
{
	public class CommentsException : Exception
	{
		public CommentsException(string name, object value, string message) : base($"Exception, operation type:{name} object: {value} has error {message}")
		{
		}
	}
}

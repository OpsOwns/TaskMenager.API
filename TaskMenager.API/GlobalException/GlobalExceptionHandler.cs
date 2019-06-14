using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using TaskMenager.API.Models;
namespace TaskMenager.API.GlobalException
{
	public class GlobalExceptionHandler : IExceptionFilter
	{
		private readonly ILogger _logger;
		public GlobalExceptionHandler(ILogger logger) => _logger = logger;
		public void OnException(ExceptionContext context)
		{
			var response = new ErrorResponse()
			{
				Message = context.Exception.Message,
				StackTrace = context.Exception.StackTrace
			};

			context.Result = new ObjectResult(response)
			{
				StatusCode = 500,
				DeclaredType = typeof(ErrorResponse)
			};
			_logger.Error($"Błąd: {context.Exception.Message}", context.Exception);
		}
	}
}

using System;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskMenagerService.Interfaces;
using TaskMenagerService.Models.TemplateTable;
using TaskMenagerService.Services.NotifyService.Queries;
namespace TaskMenager.API.Controllers
{
	[Route("Notify")]
	[ApiController]
	public class HomeController : ControllerBase
	{
		private INotifySender _userHandler;
		private ILogger<HomeController> _logger;
		readonly IMediator mediator;
		public HomeController(ILogger<HomeController> logger, INotifySender userHandler, IMediator mediator)
		{
			_logger = logger;
			_userHandler = userHandler;
			this.mediator = mediator;
		}
		[HttpGet]
		public IActionResult Notify()
		{
			var usefr = new UserCredentials() { EmailFrom = "task-manager@wp.pl", EmailTo = "test@hotmail.com", Login = "task-manager@wp.pl", Password = "blabla" };
			RecurringJob.AddOrUpdate("Powiadomienia", () => _userHandler.SendNotify(usefr, new NotifyQuery { Login = "Admin" }), "46 19 * * 1-5", TimeZoneInfo.Local);
			_logger.LogInformation("Powiadomienia zostały uruchomione");
			return Ok();
		}
	}
}

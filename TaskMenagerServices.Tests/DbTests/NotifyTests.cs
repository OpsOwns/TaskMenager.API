using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskMenagerService.DbAccess;
using TaskMenagerService.Interfaces;
using TaskMenagerService.Services.NotifyService.Queries;
using Xunit;

namespace TaskMenagerServices.Tests.DbTests
{
	public class NotifyTests
	{
		private readonly IMediator _mediator;
		ServiceCollection services;
		ITasksDbContext tasksDbContext;
		public NotifyTests()
		{
			tasksDbContext = new TasksDbContext();
			services = new ServiceCollection();
			services.AddScoped<ITasksDbContext, TasksDbContext>();
			services.AddScoped<INotifyQueryHandler, NotifyQueryHandler>();
			services.AddScoped<ILogger<NotifyQueryHandler>, Logger<NotifyQueryHandler>>();
			services.AddAutoMapper();
			services.AddLogging();
			services.AddMediatR(typeof(NotifyQueryHandler));
			_mediator = services.BuildServiceProvider().GetService<IMediator>();
		}
		[Fact]
		public async Task NotifyListTest()
		{
			var data = new NotifyQuery { Login = "Adam" };
			var result = await _mediator.Send(data);
			Assert.NotEmpty(result);
		}

	}
}

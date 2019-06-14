using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMenagerService.DbAccess;
using TaskMenagerService.Interfaces;
using TaskMenagerService.Security;
using TaskMenagerService.Services.TaskService.Commands.TaskDeleteCommand;
using TaskMenagerService.Services.TaskService.Commands.TaskSaveCommand;
using TaskMenagerService.Services.TaskService.Commands.TaskUpdateCommand;
using TaskMenagerService.Services.TaskService.Queries;
using Xunit;

namespace TaskMenagerServices.Tests.DbTests
{
	public class TasksTests
	{
		private readonly IMediator _mediator;
		ServiceCollection services;
		ITasksDbContext tasksDbContext;
		AesAlgorithm aes = new AesAlgorithm();
		public TasksTests()
		{
			tasksDbContext = new TasksDbContext();
			services = new ServiceCollection();
			services.AddScoped<ITasksDbContext, TasksDbContext>();
			services.AddScoped<ITaskSaveCommandHandler, TaskSaveCommandHandler>();
			services.AddScoped<ITaskUpdateCommandHandler, TaskUpdateCommandHandler>();
			services.AddScoped<ITaskDeleteCommandHandler, TaskDeleteCommandHandler>();
			services.AddScoped<ITasksDoneListQueryHandler, TasksDoneListQueryHandler>();
			services.AddScoped<ILogger<TaskSaveCommandHandler>, Logger<TaskSaveCommandHandler>>();
			services.AddScoped<ITaskListQueryHandler, TaskListQueryHandler>();
			services.AddAutoMapper();
			services.AddLogging();
			services.AddTransient(typeof(AesAlgorithm));
			services.AddMediatR(typeof(TaskSaveCommandHandler), typeof(TaskListQueryHandler), typeof(TasksDoneListQueryHandler), typeof(TaskDeleteCommandHandler), typeof(TaskUpdateCommandHandler));
			_mediator = services.BuildServiceProvider().GetService<IMediator>();
		}
		[Fact]
		public async Task TaskSaveCommandTest()
		{
			var task = new TaskSaveCommand().CreateTask("Janusz", "Kwiatek", false, 1, DateTime.Now, DateTime.Now);

			var dataf = JsonConvert.SerializeObject(task);
			var ares = aes.Encrypt(dataf);
			await _mediator.Send(task);
			var data = tasksDbContext.Tasks.FirstOrDefault();
			Assert.NotNull(data);
		}
		[Fact]
		public async Task TaskUpdateCommandTest()
		{
			var task = new TaskUpdateCommand().CreateTask(1, "Janusz", "ERewr", false, 1, DateTime.Now, DateTime.Now);
			await _mediator.Send(task);
			var data = tasksDbContext.Tasks.FirstOrDefault();
			Assert.Equal("Janusz", data.CurrentTask);
		}
		[Fact]
		public async Task TaskDeleteCommandTest()
		{
			var task = new TaskDeleteCommand { Task_id = 1 };
			await _mediator.Send(task);
			var data = tasksDbContext.Tasks.FirstOrDefault();
			Assert.True(data.Done);
		}

		[Fact]
		public async Task TaskListForUserTest()
		{
			var task = new TasksListQuery { User_Id = 1 };
			var result = await _mediator.Send(task);
			Assert.NotNull(result);
		}

		[Fact]
		public async Task TaskListAllTest()
		{
			var task = new TasksDoneListQuery();
			var result = await _mediator.Send(task);
			Assert.NotNull(result);
		}
	}
}

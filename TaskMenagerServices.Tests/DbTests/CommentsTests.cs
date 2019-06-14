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
using TaskMenagerService.Services.CommentService.Commands.CommentSaveCommand;
using TaskMenagerService.Services.CommentService.Commands.CommentUpdateCommand;
using Xunit;

namespace TaskMenagerServices.Tests.DbTests
{
	public class CommentsTests
	{
		private readonly IMediator _mediator;
		ServiceCollection services;
		ITasksDbContext tasksDbContext;
		AesAlgorithm aes = new AesAlgorithm();
		public CommentsTests()
		{
			tasksDbContext = new TasksDbContext();
			services = new ServiceCollection();
			services.AddScoped<ITasksDbContext, TasksDbContext>();
			services.AddScoped<ICommentSaveCommandHandler, CommentSaveCommandHandler>();
			services.AddScoped<ICommentSaveCommandHandler, CommentSaveCommandHandler>();
			services.AddScoped<ILogger<CommentSaveCommandHandler>, Logger<CommentSaveCommandHandler>>();
			services.AddAutoMapper();
			services.AddLogging();
			services.AddMediatR(typeof(CommentSaveCommandHandler), typeof(CommentUpdateCommandHandler));
			_mediator = services.BuildServiceProvider().GetService<IMediator>();
		}

		[Fact]
		public async Task InsertCommentTest()
		{
			var data = new CommentSaveCommand().CreateComment("Blabla", 16, 2, DateTime.Now);
			var json = JsonConvert.SerializeObject(data);
			var resultr = aes.Encrypt(json);
			await _mediator.Send(data);
			var result = tasksDbContext.Comments.First();
			Assert.NotNull(result);
		}
		[Fact]
		public async Task UpdateCommentTest()
		{
			var data = new CommentUpdateCommand().CreateComment(2,2, "Chuj");
			var json = JsonConvert.SerializeObject(data);
			var resultr = aes.Encrypt(json);
			await _mediator.Send(data);
			var result = tasksDbContext.Comments.First();
			Assert.Equal("Testowanie aplikacji", result.Comment);
		}
	}
}

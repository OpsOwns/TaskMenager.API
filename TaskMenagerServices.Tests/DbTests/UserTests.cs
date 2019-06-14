using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskMenagerService.DbAccess;
using TaskMenagerService.Interfaces;
using TaskMenagerService.Models;
using TaskMenagerService.Security;
using TaskMenagerService.Services.UserService.Commands.UserDeleteCommand;
using TaskMenagerService.Services.UserService.Commands.UserUpdateCommand;
using TaskMenagerService.Services.UserService.Queires;
using TaskMenagerService.Services.UserService.Queries;
using TaskMenagerService.UserService.Commands;
using Xunit;

namespace TaskMenagerServices.Tests.DbTests
{
	public class UserTests
	{
		private readonly IMediator _mediator;
		ServiceCollection services;
		ITasksDbContext tasksDbContext;
		private IAesAlgorithm aes;
		public UserTests()
		{
			tasksDbContext = new TasksDbContext();
			aes = new AesAlgorithm();
			services = new ServiceCollection();
			services.AddScoped<ITasksDbContext, TasksDbContext>();
			services.AddEntityFrameworkSqlServer();
			services.AddTransient<IAesAlgorithm, AesAlgorithm>();
			services.AddScoped<ILogger<UserSaveCommandHandler>, Logger<UserSaveCommandHandler>>();
			services.AddScoped<ILogger<UserUpdateCommandHandler>, Logger<UserUpdateCommandHandler>>();
			services.AddScoped<ILogger<UserDeleteCommandHandler>, Logger<UserDeleteCommandHandler>>();
			services.AddAutoMapper();
			services.AddLogging();
			services.AddTransient(typeof(AesAlgorithm));
			services.AddScoped<IUserLoginQueryHandler, UserLoginQueryHandler>();
			services.AddScoped<IUserSaveCommandHandler, UserSaveCommandHandler>();
			services.AddScoped<IUserUpdateCommandHandler, UserUpdateCommandHandler>();
			services.AddScoped<IUserDeleteCommandHandler, UserDeleteCommandHandler>();
			services.AddScoped<IUserListQueryHandler, UserListQueryHandler>();
			services.AddMediatR(typeof(UserSaveCommandHandler), typeof(UserUpdateCommandHandler), typeof(UserUpdateCommand), typeof(UserDeleteCommandHandler), typeof(UserListQueryHandler));
			_mediator = services.BuildServiceProvider().GetService<IMediator>();
		}
		[Fact]
		public void VerifyPassword()
		{
			var result = tasksDbContext.Users.First(n => n.Login == "Laura").Password;
			var test = result.VerifyPassword("wolski");
			Assert.True(test);
		}

		[Fact]
		public async Task GetUserListTest()
		{
			var login = new UserListQuery().CreateList("Janusz");
			var result = await _mediator.Send(login);
			Assert.NotEmpty(result);
		}

		[Fact]
		public void AesCryptoTest()
		{
			string value = "Rak";
			string encryptedText = aes.Encrypt(value);
			var result = aes.Decrypt(encryptedText);
			Assert.Equal("Rak", result);
		}
		[Fact]
		public async Task LoginTest()
		{

			var user = new UserLoginQuery().CreateQuery("Laura", "wolski");
			var obj = JsonConvert.SerializeObject(user);
			var data = aes.Encrypt(obj);
			var result = await _mediator.Send(user);
			Assert.NotNull(result);
		}

		[Fact]
		public async Task CheckHashPassword()
		{
			var user = new UserSaveCommand().CreateUser("Laura", "wolski".Encrypt(), "ttest", "G");
			await _mediator.Send(user);
			Assert.True(true);
		}
		[Fact]
		public async Task InsertUserTest()
		{

			var user = new UserSaveCommand().CreateUser("Admin", "admin123".Encrypt(), "krzysztof.papciak@hotmail.com", "A");

			var data = JsonConvert.SerializeObject(user);
			var aesdata = aes.Encrypt(data);
			await _mediator.Send(user);
			var result = tasksDbContext.Users.First(n => n.Login == "Baca").Login;
			Assert.Equal("Baca", result);
		}

		[Fact]
		public async Task DeletetUserTest()
		{
			var user = new UserDeleteCommand { User_Id = 18 };
			await _mediator.Send(user);
			var result = tasksDbContext.Users.First(n => n.User_Id == 18).LockAccount;
			Assert.True(result);
		}

		[Fact]
		public async Task UpdateUserTest()
		{
			var user = new UserUpdateCommand().CreateUser("Jurek", "Data", "f", "G");
			user.User_Id = 18;

			var data = JsonConvert.SerializeObject(user);
			var aesdata = aes.Encrypt(data);
			await _mediator.Send(user);
			var result = tasksDbContext.Users.FirstOrDefault(p => p.Login == "Hiena").Password;
			Assert.Equal("jarek", result);
		}
	}
}

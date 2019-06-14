using AutoMapper;
using FluentValidation;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using TaskMenager.API.GlobalException;
using TaskMenager.API.HangFire;
using TaskMenagerService.DbAccess;
using TaskMenagerService.Extensions.Configuration;
using TaskMenagerService.Interfaces;
using TaskMenagerService.Models;
using TaskMenagerService.Security;
using TaskMenagerService.Services.CommentService.Commands.CommentSaveCommand;
using TaskMenagerService.Services.CommentService.Commands.CommentUpdateCommand;
using TaskMenagerService.Services.NotifyService.Notify;
using TaskMenagerService.Services.NotifyService.Queries;
using TaskMenagerService.Services.TaskService.Commands.TaskDeleteCommand;
using TaskMenagerService.Services.TaskService.Commands.TaskSaveCommand;
using TaskMenagerService.Services.TaskService.Commands.TaskUpdateCommand;
using TaskMenagerService.Services.TaskService.Queries;
using TaskMenagerService.Services.UserService.Commands.UserDeleteCommand;
using TaskMenagerService.Services.UserService.Commands.UserUpdateCommand;
using TaskMenagerService.Services.UserService.Queries;
using TaskMenagerService.UserService.Commands;
using TaskMenagerService.Validation.Users;

namespace TaskMenager.API
{
	public class Startup
	{
		const string _connectionString = @"Data Source=DESKTOP-7GLL7LJ\OPSDB;Initial Catalog=TasksERP;Integrated Security=True";
		public Startup(IConfiguration configuration) => Configuration = configuration;
		public IConfiguration Configuration { get; }
		private ILoggerFactory _loggerFactory;
		public void ConfigureServices(IServiceCollection services)
		{
			#region Database Token
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer((options) =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = Configuration.GetValidIssuer(),
					ValidAudience = Configuration.GetValidAudience(),
					IssuerSigningKey = Configuration.GetSymmetricSecurityKey()
				};
			});
			services.AddEntityFrameworkSqlServer();
			services.AddScoped<ITokenGenerator, TokenGenerator>();
			services.AddScoped<ITasksDbContext, TasksDbContext>();
			#endregion
			#region other stuff
			services.AddScoped<IAesAlgorithm, AesAlgorithm>();
			services.AddScoped<INotifySender, NotifySender>();
			services.AddTransient<IValidator<UserSaveCommand>, UserValidation>();
			services.AddTransient<IValidator<TaskSaveCommand>, TasksValidation>();
			services.AddTransient<IValidator<CommentSaveCommand>, CommentsValidation>();
			services.AddAutoMapper();
			services.AddMediatR(typeof(UserSaveCommandHandler), typeof(NotifyQueryHandler), typeof(UserUpdateCommandHandler), typeof(UserDeleteCommandHandler), typeof(UserLoginQueryHandler), typeof(UserListQueryHandler), typeof(TaskSaveCommandHandler), typeof(TaskUpdateCommandHandler), typeof(TaskDeleteCommandHandler), typeof(TaskListQueryHandler), typeof(TasksDoneListQueryHandler), typeof(CommentSaveCommandHandler), typeof(CommentUpdateCommandHandler));
			#endregion
			#region Logger
			services.AddLogging(loginBuilder =>
			{
				loginBuilder.AddConfiguration(Configuration.GetSection("Logging"));
				loginBuilder.AddConsole();
				loginBuilder.AddDebug();
				loginBuilder.AddSerilog();
			});
			Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.RollingFile(@"Logs\log-{Date}.txt", Serilog.Events.LogEventLevel.Information, "[{Timestamp:HH:mm} {Level:u3}] {Message:lj}{NewLine}{Exception}").CreateLogger();
			#endregion
			#region HangFire
			services.AddHangfire(x => x.UseSqlServerStorage(_connectionString));
			services.AddHangfireServer();
			#endregion
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddMvcOptions((cfg) => cfg.Filters.Add(new GlobalExceptionHandler(Log.Logger)));
		}
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			else
				app.UseHsts();
			_loggerFactory = logger;
			app.UseAuthentication();
			app.UseHttpsRedirection();
			app.UseHangfireDashboard("/jobs", new DashboardOptions()
			{
				Authorization = new[] { new HangFireAuthorizationFilter(Configuration) }
			});
			app.UseMvc();
		}
	}
}

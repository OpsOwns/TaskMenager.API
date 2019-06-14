using FluentValidation;
using TaskMenagerService.Services.TaskService.Commands.TaskSaveCommand;

namespace TaskMenagerService.Validation.Users
{
	public class TasksValidation : AbstractValidator<TaskSaveCommand>
	{
		public TasksValidation()
		{
			RuleFor(task => task.CurrentTask).NotEmpty().NotNull().WithMessage("Wartość nie może być pusta");
			RuleFor(task => task.UserDescription).NotEmpty().NotNull().WithMessage("Wartość nie może być pusta");
			RuleFor(task => task.DateCreate).NotEmpty().NotNull().WithMessage("Wartość nie może być pusta");
			RuleFor(task => task.FK_User_Id).Must((x) => x > 0).NotNull().WithMessage("Wartość nie może być pusta");
			RuleFor(task => task.DateEnd).NotEmpty().NotNull().WithMessage("Wartość nie może być pusta");
		}
	}
}

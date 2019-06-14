using FluentValidation;
using TaskMenagerService.Services.CommentService.Commands.CommentSaveCommand;
namespace TaskMenagerService.Validation.Users
{
	public class CommentsValidation : AbstractValidator<CommentSaveCommand>
	{
		public CommentsValidation()
		{
			RuleFor(com => com.Comment).NotNull().NotEmpty().WithMessage("Wartość nie może być pusta");
			RuleFor(com => com.Comment).NotNull().NotEmpty().WithMessage("Wartość nie może być pusta");
			RuleFor(com => com.DateCreate).NotNull().NotEmpty().WithMessage("Wartość nie może być pusta");
			RuleFor(com => com.FK_Users_Id).NotNull().Must((x) => x > 0).WithMessage("Wartość nie może być pusta");
			RuleFor(com => com.FK_Task_Id).NotNull().Must((x) => x > 0).WithMessage("Wartość nie może być pusta");
		}
	}
}

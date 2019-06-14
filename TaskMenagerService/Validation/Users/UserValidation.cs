using FluentValidation;
using TaskMenagerService.Models;
namespace TaskMenagerService.Validation.Users
{
	public class UserValidation : AbstractValidator<UserSaveCommand>
	{
		public UserValidation()
		{
			RuleFor(log => log.Login).Matches(@"^[\w.-0-9]{6,13}$").WithMessage("Proszę podać przynajmniej 6 znaków").NotEmpty();
			RuleFor(ema => ema.Email).EmailAddress().WithMessage("Proszę wpisać adres e-mail").NotEmpty();
		}
	}
}

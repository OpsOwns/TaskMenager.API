using TaskMenagerService.Validation.Users;
using FluentValidation.TestHelper;
using Xunit;
using TaskMenagerService.Models;

namespace TaskMenagerServices.Tests.ValidationTests
{
	public class UserValidationTests
	{
		private UserValidation validations;
		private UserSaveCommand userDTO;

		[Fact]
		public void Execute()
		{
			validations = new UserValidation();
			userDTO = new UserSaveCommand();
		}

		[Fact]
		public void LoginFailTest()
		{
			Execute();
			validations.ShouldHaveValidationErrorFor(login => login.Login, userDTO.Login);
		}
		[Fact]
		public void Should_not_have_error_when_name_is_specified()
		{
			Execute();
			validations.ShouldNotHaveValidationErrorFor(person => person.Login, "JeremyOwns");
		}
		[Theory]
		[InlineData("JeremyOwns")]
		[InlineData("AdminPro9943")]
		[InlineData("Har..as_99Abr")]
		public void Auto_Login_Validation(string login)
		{
			Execute();
			validations.ShouldNotHaveValidationErrorFor(person => person.Login, login);
		}

		[Theory]
		[InlineData("Admin0042312")]
		[InlineData("dzGFFiubex_@o2.pl")]
		public void Password_Checker_Test(string password)
		{
			Execute();
			validations.ShouldNotHaveValidationErrorFor(person => person.Password, password);
		}
		[Theory]
		[InlineData("jadziababcia")]
		public void Password_Fail_Test(string password)
		{
			Execute();
			validations.ShouldHaveValidationErrorFor(person => person.Password, password);
		}

		[Theory]
		[InlineData("Jebzdzido@gmail.com")]
		[InlineData("Rak@o2.pl")]
		public void Email_Checker_Test(string email)
		{
			Execute();
			validations.ShouldNotHaveValidationErrorFor(person => person.Email, email);
		}

		[Fact]
		public void Email_Fail_Test()
		{
			Execute();
			validations.ShouldHaveValidationErrorFor(person => person.Email, userDTO.Email);
		}


	}
}

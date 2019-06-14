using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaskMenagerService.Models.TemplateTable
{
	public class Users
	{
		[Key]
		public int User_Id { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string Flag { get; set; }
		public DateTime DateCreate { get; set; }
		public bool LockAccount { get; set; }
		public ICollection<Tasks> Tasks { get; set; }
		public ICollection<Comments> Comments { get; set; }
	}
}

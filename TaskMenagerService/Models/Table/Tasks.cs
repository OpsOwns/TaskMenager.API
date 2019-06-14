using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TaskMenagerService.Models.TemplateTable
{
	public class Tasks
	{
		[Key]
		public int Task_Id { get; set; }
		public string CurrentTask { get; set; }
		public string UserDescription { get; set; }
		public bool Done { get; set; }
		[ForeignKey("FK_User_Id")]
		public Users User { get; set; }
		public int FK_User_Id { get; set; }
		public DateTime DateCreate { get; set; }
		public DateTime DateEnd { get; set; }
		public ICollection<Comments> Comments { get; set; }
	}
}

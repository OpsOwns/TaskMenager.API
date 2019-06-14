using System;
using System.ComponentModel.DataAnnotations;

namespace TaskMenagerService.Models.TemplateTable
{
	public class TasksComments
	{
		[Key]
		public int Task_Id { get; set; }
		public string CurrentTask { get;  set; }
		public string UserDescription { get;  set; }
		public string Login { get; set; }
		public bool Done { get;  set; }
		public string Comment { get;  set; }
		public DateTime DateCreate { get;  set; }
		public DateTime DateEnd { get;  set; }
	}
}

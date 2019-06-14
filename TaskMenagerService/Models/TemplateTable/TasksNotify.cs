using System;
using System.ComponentModel.DataAnnotations;

namespace TaskMenagerService.Models.TemplateTable
{
	public class TasksNotify
	{
		[Key]
		public string CurrentTask { get; set; }
		public string Comment { get; set; }
		public string Login { get; set; }
		public DateTime DateEnd { get; set; }
		public DateTime DateCreate { get; set; }
	}
}

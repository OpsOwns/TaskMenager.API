using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TaskMenagerService.Models.TemplateTable
{
	public class Comments
	{
		[Key]
		public int Comment_Id { get; set; }
		public string Comment { get; set; }
		[ForeignKey("FK_Users_Id")]
		public Users User { get; set; }
		public int FK_Users_Id { get; set; }
		[ForeignKey("FK_Task_Id")]
		public Tasks Task { get; set; }
		public int FK_Task_Id { get; set; }
		public DateTime DateCreate { get; set; }
	}
}

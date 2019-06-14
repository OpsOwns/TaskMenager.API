using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskMenagerService.Models.TemplateTable
{
	public class UserCredentials
	{
		public string EmailFrom { get; set; }
		public string EmailTo { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public string Body { get; set; }

	}
}

using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TaskMenagerService.Interfaces;
using TaskMenagerService.Models.TemplateTable;
using TaskMenagerService.Services.NotifyService.Queries;

namespace TaskMenagerService.Services.NotifyService.Notify
{
	public class NotifySender : INotifySender
	{
		private readonly IMediator _mediatr;
		private readonly MailMessage mailMessage;
		private readonly SmtpClient smtpClient;
		private readonly ILogger<NotifyQuery> _logger;
		public NotifySender(IMediator mediatr, ILogger<NotifyQuery> logger)
		{
			mailMessage = new MailMessage();
			smtpClient = new SmtpClient();
			_mediatr = mediatr;
			_logger = logger;
		}
		public async Task SendNotify(UserCredentials user, NotifyQuery notify)
		{
			var notifyBody = await GetNotifies(notify);
			user.Body = SetHtmlTable(notifyBody);
			ConfigurationMail(user);
			await smtpClient.SendMailAsync(mailMessage);
			_logger.LogInformation($"Email sended at {DateTime.Now.ToString("HH:MM")}");
		}

		private string SetHtmlTable(List<NotifyQuery> notifyQueryList)
		{
			try
			{
				string messageBody = "<font>Zrealizowane zadania: </font><br><br>";
				string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center;\" >";
				string htmlTableEnd = "</table>";
				string htmlHeaderRowStart = "<tr style =\"background-color:#6FA1D2; color:#ffffff;\">";
				string htmlHeaderRowEnd = "</tr>";
				string htmlTrStart = "<tr style =\"color:#555555;\">";
				string htmlTrEnd = "</tr>";
				string htmlTdStart = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">";
				string htmlTdEnd = "</td>";

				messageBody += htmlTableStart;
				messageBody += htmlHeaderRowStart;
				messageBody += htmlTdStart + "Lista zrealizowanych zadań" + htmlTdEnd;
				messageBody += htmlHeaderRowEnd;

				foreach (var item in notifyQueryList)
				{
					messageBody = messageBody + htmlTrStart;
					messageBody = messageBody + htmlTdStart + $"{item.Login} | {item.CurrentTask} |  {item.Comment} |  {item.DateCreate.ToString("dd.MM.yyyy HH:MM")} |  {item.DateEnd.ToString("dd.MM.yyyy HH:MM")} " + htmlTdEnd;
					messageBody = messageBody + htmlTrEnd;
				}
				messageBody = messageBody + htmlTableEnd;
				return messageBody;
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error {ex.Message}");
				throw ex;
			}
		}
		private async Task<List<NotifyQuery>> GetNotifies(NotifyQuery notify) => await _mediatr.Send(notify);
		private void ConfigurationMail(UserCredentials user)
		{
			string email = user.EmailFrom;
			string emailTo = user.EmailTo;
			string password = user.Password;
			string title = $"Zrealizowane zadania {DateTime.Today.Date.ToString("dd.MM.yyyy")}";
			string name = user.Login;
			mailMessage.To.Add(emailTo);
			mailMessage.From = new MailAddress(email, name);
			mailMessage.Subject = title;
			mailMessage.Body = user.Body;
			mailMessage.BodyEncoding = Encoding.UTF8;
			mailMessage.IsBodyHtml = true;
			smtpClient.EnableSsl = true;
			smtpClient.Host = "smtp.wp.pl";
			smtpClient.Port = 587;
			smtpClient.Credentials = new NetworkCredential(email, password);
			smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
		}
	}
}

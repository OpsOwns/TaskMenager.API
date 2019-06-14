using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.Extensions.Configuration;
namespace TaskMenager.API.HangFire
{
	public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
	{
		IConfiguration _configuration;
		public HangFireAuthorizationFilter(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public bool Authorize([NotNull] DashboardContext context)
		{
			var httpContext = context.GetHttpContext();
			var userToken = httpContext.Request.Cookies["accessToken"];
			var configToken = _configuration.GetValue<string>("HangFire:SecretKey");
			return configToken.Equals(userToken) ? true : false;
		}
	}
}

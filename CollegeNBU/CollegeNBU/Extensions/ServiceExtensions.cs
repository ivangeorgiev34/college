using Microsoft.AspNetCore.Identity.UI.Services;

namespace CollegeNBU.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IEmailSender, NoOpEmailSender>();
        }
    }
}

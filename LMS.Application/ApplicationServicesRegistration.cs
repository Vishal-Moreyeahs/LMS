using System.Reflection;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Services.CompanyServices;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddTransient<ICompanyRepository, CompanyRepository>();

            return services;
        }
    }
}

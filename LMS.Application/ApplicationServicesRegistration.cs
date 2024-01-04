using System.Reflection;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Services.AdminServices;
using LMS.Application.Services.CompanyServices;
using LMS.Application.Services.DomainServices;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient<IDomainRepository, DomainRepository>();
            services.AddTransient<ISubDomainRepository, SubDomainRepository>();

            return services;
        }
    }
}

using System.Reflection;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Services.AdminServices;
using LMS.Application.Services.CompanyServices;
using LMS.Application.Services.DomainServices;
using LMS.Application.Services.RoleManager;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddTransient<ICompanyRepository, CompanyServices>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IDomainServices, DomainServices>();
            services.AddTransient<IRoleServices,RoleServices>();

            return services;
        }
    }
}

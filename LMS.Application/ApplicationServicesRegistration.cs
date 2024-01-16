using System.Reflection;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Services.AdminServices;
using LMS.Application.Services.CompanyServices;
using LMS.Application.Services.CourseManager;
using LMS.Application.Services.DomainServices;
using LMS.Application.Services.FileBankManager;
using LMS.Application.Services.QuestionBankManager;
using LMS.Application.Services.QuizManager;
using LMS.Application.Services.QuizQuestionManager;
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
            services.AddTransient<IFileBankServices, FileBankServices>();
            services.AddTransient<ICourseServices, CourseServices>();
            services.AddTransient<IQuestionBankServices, QuestionBankServices>();
            services.AddTransient<IQuizServices, QuizServices>();
            services.AddTransient<IQuizQuestionServices, QuizQuestionServices>();

            return services;
        }
    }
}

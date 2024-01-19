using AutoMapper;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Application.Response;
using LMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Services.EmployeeCourseManager
{
    public class EmployeeCourseServices : IEmployeeCourseServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public EmployeeCourseServices(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService)
        { 
            _authenticatedUserService = authenticatedUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<dynamic>> AssignCourseToEmployee(List<EmployeeCourseRequest> employeeCourses)
        {
            if (employeeCourses.Count == 0 && employeeCourses == null)
            {
                throw new ApplicationException("Invalid Request");
            }

            var data = _mapper.Map<List<EmployeeCourse>>(employeeCourses);

            await _unitOfWork.GetRepository<EmployeeCourse>().AddRange(data); 
            var isDataAdded = await _unitOfWork.SaveChangesAsync();
            if (isDataAdded <= 0)
            {
                throw new ApplicationException("Unable to assign course to employees");
            }
            var response = new Response<dynamic>
            { 
                Status = true,
                Message = "Successfully added the course to employees"
            };
            return response;
        }

        public async Task<Response<dynamic>> DeleteCourseForEmployee(int employeeCourseId)
        {
            var employeeCourse = await _unitOfWork.GetRepository<EmployeeCourse>().Get(employeeCourseId);
            if (employeeCourse == null || !employeeCourse.IsActive)
            {
                throw new ApplicationException("Employee course not found");
            }

            employeeCourse.IsActive = false;
            await _unitOfWork.GetRepository<EmployeeCourse>().Update(employeeCourse);
            var isEmployeeCourseDeleted = await _unitOfWork.SaveChangesAsync();

            if (isEmployeeCourseDeleted <= 0)
            {
                throw new ApplicationException("Employee Course not deleted");
            }

            var response = new Response<dynamic>
            {
                Status = true,
                Message = "Employee Course deleted successfully",
            };
            return response;
        }

        public async Task<Response<dynamic>> GetCoursesByEmployeeId(int employeeId)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            var employee = await _unitOfWork.GetRepository<Employee>().Get(employeeId);

            if (employee == null || !employee.IsActive || employee.Company_Id != loggedInUser.CompanyId)
            {
                throw new ApplicationException("Employee not found");
            }

            var quizQuestion = _unitOfWork.GetRepository<EmployeeCourse>().GetAllRelatedEntity();

            quizQuestion = quizQuestion.Where(x => x.Employee_Id == employeeId && x.IsActive).ToList();

            var questions = quizQuestion.Select(x => x.Courses).ToList();

            var response = new Response<dynamic>
            {
                Status = true,
                Message = "Question for Quiz is retreived successfully",
                Data = questions
            };
            return response;
        }
    }
}

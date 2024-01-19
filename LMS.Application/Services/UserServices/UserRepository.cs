using System.Data;
using AutoMapper;
using LMS.Application.Contracts.Infrastructure;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Application.Response;
using LMS.Domain.Enums;
using LMS.Domain.Models;

namespace LMS.Application.Services.AdminServices
{
    public class UserRepository : IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICryptographyService _cryptographyService;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IEmployeeCourseServices _employeeCourseServices;
        private readonly ICourseServices _courseServices;
        public UserRepository(IMapper mapper, IAuthenticatedUserService authenticatedUserService,ICryptographyService cryptographyService
                                ,IUnitOfWork unitOfWork, ICourseServices courseServices, IEmployeeCourseServices employeeCourseServices)
        {
            _mapper = mapper;
            _authenticatedUserService = authenticatedUserService;
            _unitOfWork = unitOfWork;
            _cryptographyService = cryptographyService;
            _employeeCourseServices = employeeCourseServices;
            _courseServices = courseServices;
        }
        public async Task<Response<List<RegistrationResponse>>> AddUser(List<RegistrationRequest> users)
        {
            if (users == null)
            {
                throw new ApplicationException($"Invalid Request");
            }

            var allCourses = await _courseServices.GetAllCourse();
            var courses = allCourses.Data.Where(x => x.IsMandatory).ToList();

            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            foreach (var user in users)
            {
                var existingUser = _unitOfWork.GetRepository<Employee>().GetAll().Result.Where(x => x.Email == user.Email && x.IsActive).ToList();
                if (!(existingUser.Count > 0))
                { 
                    await Register(user, courses);
                }
            }

            var response = new Response<List<RegistrationResponse>>
            {
                Status = true,
                Message = "Users Data Added Successfully"
            };

            return response;
            
        }

        public async Task<Response<UserData>> DeleteUser(int userId)
        {
            var userDetails = await _unitOfWork.GetRepository<Employee>().Get(userId);

            if (userDetails == null || userDetails.Role_Id != (int)RoleEnum.Admin)
            {
                throw new ApplicationException($"User with id - {userId} not exists");
            }

            userDetails.IsActive = false;
            var result = _unitOfWork.GetRepository<Employee>().Update(userDetails);
            await _unitOfWork.SaveChangesAsync(); 

            var response = new Response<UserData>
            {
                Status = true,
                Message = $"User With Id - {userId} deleted Successfully",
                Data = _mapper.Map<UserData>(userDetails)
            };

            return response;
        }

        public async Task<Response<UserData>> GetUserById(int userId)
        {
            var userDetails = await _unitOfWork.GetRepository<Employee>().Get(userId);

            if (userDetails == null || userDetails.Role_Id != (int)RoleEnum.Admin)
            {
                throw new ApplicationException($"User with id - {userId} not exists");
            }

            var response = new Response<UserData>
            {
                Status = true,
                Message = $"User With Id - {userId} Retreived Successfully",
                Data = _mapper.Map<UserData>(userDetails)
            };

            return response;
        }

        public async Task<Response<List<UserData>>> GetAllUser()
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            var employeeList = await _unitOfWork.GetRepository<Employee>().GetAll();

            if (loggedInUser.RoleId == (int)RoleEnum.SuperAdmin)
            {
                employeeList = employeeList.Where(x => x.IsActive && x.CreatedBy == loggedInUser.EmployeeId && x.Id != loggedInUser.EmployeeId);
            }
            else
            { 
                //var list = employeeList.ToList();
                employeeList = employeeList.Where(x => x.Company_Id == loggedInUser.CompanyId && x.IsActive && x.Id != loggedInUser.EmployeeId).ToList();
                if (employeeList == null)
                {
                    throw new ApplicationException($"Data not found");
                }
            }

            var response = new Response<List<UserData>>
            {
                Status = true,
                Message = $"User retrieved successfully",
                Data = _mapper.Map<List<UserData>>(employeeList.ToList())
            };

            return response;
        }

        public async Task<Response<UserData>> UpdateUser(UserData admin)
        {
            var adminData = await _unitOfWork.GetRepository<Employee>().Get(admin.Id);

            if (adminData == null || adminData.Role_Id != (int)RoleEnum.Admin)
            {
                throw new ApplicationException($"User with id - {admin.Id} not exists");
            }

            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            _mapper.Map(admin, adminData);
            //adminData.UpdatedDate = DateTime.UtcNow;
            //adminData.UpdatedBy = loggedInUser.EmployeeId;

            var result = _unitOfWork.GetRepository<Employee>().Update(adminData);
            await _unitOfWork.SaveChangesAsync();

            var response = new Response<UserData>
            {
                Status = true,
                Message = $"User with id - {admin.Id} Updated Successfully",
                Data = admin
            };

            return response;
        }

        private async Task Register(RegistrationRequest request, List<CourseDTO> courses)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            if (loggedInUser.RoleId != (int)RoleEnum.SuperAdmin)
            {
                if (request.Company_id != loggedInUser.CompanyId)
                {
                    return;
                }
            }
            var user = _mapper.Map<Employee>(request);
            user.Password = _cryptographyService.EncryptPassword(request.Email + request.RealPassword);

            await _unitOfWork.GetRepository<Employee>().Add(user);
            var isDataAdded = await _unitOfWork.SaveChangesAsync();

            if (courses != null && courses.Count > 0)
            { 
                var listCourseAssignment = new List<EmployeeCourseRequest>();

                foreach (var course in courses)
                {
                    listCourseAssignment.Add(new EmployeeCourseRequest { Employee_Id = user.Id , Courses_Id = course.Id});
                }

                var isCoursesAssigned = await _employeeCourseServices.AssignCourseToEmployee(listCourseAssignment);

                if (!isCoursesAssigned.Status)
                {
                    throw new ApplicationException($"Courses not assigned for user {user.FirstName}");
                }
            }
            if (isDataAdded <= 0)
            {
                throw new ApplicationException($"User {user.Email} should not be added");
            }

        }
    }
}

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
        public UserRepository(IMapper mapper, IAuthenticatedUserService authenticatedUserService,ICryptographyService cryptographyService
                                ,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _authenticatedUserService = authenticatedUserService;
            _unitOfWork = unitOfWork;
            _cryptographyService = cryptographyService;
        }
        public async Task<Response<List<RegistrationResponse>>> AddUser(List<RegistrationRequest> users)
        {
            if (users == null)
            {
                throw new ApplicationException($"Invalid Request");
            }
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            foreach (var user in users)
            {
                var existingUser = _unitOfWork.GetRepository<Employee>().GetAll().Result.Where(x => x.Email == user.Email).ToList();
                if (!(existingUser.Count > 0))
                { 
                    await Register(user);
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
            await _unitOfWork.Save(); 

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
            var employeeList = await _unitOfWork.GetRepository<Employee>().GetAll();

            var adminData = employeeList.Where(x => x.Role_Id == (int)RoleEnum.Admin).ToList();

            if (adminData != null)
            {
                throw new ApplicationException($"Data not found");
            }

            var response = new Response<List<UserData>>
            {
                Status = true,
                Message = $"User retrieved successfully",
                Data = _mapper.Map<List<UserData>>(adminData.ToList())
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
            adminData.UpdatedDate = DateTime.UtcNow;
            adminData.UpdatedBy = loggedInUser.EmployeeId;

            var result = _unitOfWork.GetRepository<Employee>().Update(adminData);
            await _unitOfWork.Save();

            var response = new Response<UserData>
            {
                Status = true,
                Message = $"User with id - {admin.Id} Updated Successfully",
                Data = admin
            };

            return response;
        }

        private async Task Register(RegistrationRequest request)
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
            user.CreatedDate = DateTime.UtcNow;
            user.UpdatedDate = DateTime.UtcNow;
            user.CreatedBy = loggedInUser.EmployeeId;
            user.UpdatedBy = loggedInUser.EmployeeId;
            user.Password = _cryptographyService.EncryptPassword(request.Email + request.RealPassword);

            var registerUser = await _unitOfWork.GetRepository<Employee>().Add(user);
            var isDataAdded = await _unitOfWork.Save();
            if (isDataAdded <= 0)
            {
                throw new ApplicationException($"User {user.Email} should not be added");
            }
        }
    }
}

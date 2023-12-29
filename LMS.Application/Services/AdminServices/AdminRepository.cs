using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Application.Contracts.Infrastructure;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Application.Response;
using LMS.Domain.Enums;
using LMS.Domain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LMS.Application.Services.AdminServices
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IAuthService _authService;
        private readonly IGenericRepository<Employee> _admin;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public AdminRepository(IAuthService authService, IMapper mapper, IGenericRepository<Employee> admin, IAuthenticatedUserService authenticatedUserService)
        {
            _authService = authService;
            _mapper = mapper;
            _admin = admin;
            _authenticatedUserService = authenticatedUserService;
        }
        public async Task<ApiResponse<RegistrationResponse>> AddAdmin(RegistrationRequest admin)
        {
            try
            {
                var result = await _authService.Register(admin);
                if (result == null) 
                {
                    return new ApiResponse<RegistrationResponse>
                    {
                        Status = false,
                        Message = $"Admin - {admin.Email} not added successully"
                    };
                }
                var response =  new ApiResponse<RegistrationResponse>
                {
                    Status = false,
                    Message = $"Admin - {admin.Email} added successully",
                    Data = result
                };
                return response;
            }
            catch (Exception ex)
            {
                return new ApiResponse<RegistrationResponse>
                {
                    Status = false,
                    Message = $"An error occured when adding admin !!"
                };
            }
        }

        public async Task<ApiResponse<AdminData>> DeleteAdmin(int adminId)
        {
            var adminDetails = await _admin.Get(adminId);

            if (adminDetails == null || adminDetails.Role_Id != (int)RoleEnum.Admin)
            {
                return new ApiResponse<AdminData>
                {
                    Status = false,
                    Message = $"Admin with id - {adminId} not exists"
                };
            }

            var result = _admin.Delete(adminDetails);

            var response = new ApiResponse<AdminData>
            {
                Status = true,
                Message = $"Admin With Id - {adminId} deleted Successfully",
                Data = _mapper.Map<AdminData>(adminDetails)
            };

            return response;
        }

        public async Task<ApiResponse<AdminData>> GetAdminById(int adminId)
        {
            var adminDetails = await _admin.Get(adminId);

            if (adminDetails == null || adminDetails.Role_Id != (int)RoleEnum.Admin)
            {
                return new ApiResponse<AdminData>
                {
                    Status = false,
                    Message = $"Admin with id - {adminId} not exists"
                };
            }

            var response = new ApiResponse<AdminData>
            {
                Status = true,
                Message = $"Admin With Id - {adminId} Retreived Successfully",
                Data = _mapper.Map<AdminData>(adminDetails)
            };

            return response;
        }

        public async Task<ApiResponse<List<AdminData>>> GetAllAdmin()
        {
            var employeeList = await _admin.GetAll();

            var adminData = employeeList.Where(x => x.Role_Id == (int)RoleEnum.Admin).ToList();

            if (adminData == null)
            {
                return new ApiResponse<List<AdminData>>
                {
                    Status = true,
                    Message = $"No Data Found"
                };
            }

            var response = new ApiResponse<List<AdminData>>
            {
                Status = true,
                Message = $"Companies retrieved successfully",
                Data = _mapper.Map<List<AdminData>>(adminData.ToList())
            };

            return response;
        }

        public async Task<ApiResponse<AdminData>> UpdateAdmin(AdminData admin)
        {
            var adminData = await _admin.Get(admin.Id);

            if (adminData == null || adminData.Role_Id != (int)RoleEnum.Admin)
            {
                return new ApiResponse<AdminData>
                {
                    Status = false,
                    Message = $"Admin with id - {admin.Id} not exists"
                };
            }

            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            _mapper.Map(admin, adminData);
            adminData.UpdatedDate = DateTime.UtcNow;
            adminData.UpdatedBy = loggedInUser.EmployeeId;

            var result = _admin.Update(adminData);

            var response = new ApiResponse<AdminData>
            {
                Status = true,
                Message = $"Company with id - {admin.Id} Updated Successfully",
                Data = admin
            };

            return response;
        }
    }
}

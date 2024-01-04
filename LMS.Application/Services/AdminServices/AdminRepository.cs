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
    public class AdminRepository : IAdminRepository
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public AdminRepository(IAuthService authService, IMapper mapper, IAuthenticatedUserService authenticatedUserService
                                ,IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _mapper = mapper;
            _authenticatedUserService = authenticatedUserService;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response<RegistrationResponse>> AddAdmin(RegistrationRequest admin)
        {
            try
            {
                var result = await _authService.Register(admin);
                if (result == null) 
                {
                    throw new ApplicationException($"Admin - {admin.Email} not added successully");
                }
                var response =  new Response<RegistrationResponse>
                {
                    Status = true,
                    Message = $"Admin - {admin.Email} added successully",
                    Data = result.Data
                };
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Response<AdminData>> DeleteAdmin(int adminId)
        {
            var adminDetails = await _unitOfWork.GetRepository<Employee>().Get(adminId);

            if (adminDetails == null || adminDetails.Role_Id != (int)RoleEnum.Admin)
            {
                throw new ApplicationException($"Admin with id - {adminId} not exists");
            }

            adminDetails.IsActive = false;
            var result = _unitOfWork.GetRepository<Employee>().Update(adminDetails);
            await _unitOfWork.Save(); 

            var response = new Response<AdminData>
            {
                Status = true,
                Message = $"Admin With Id - {adminId} deleted Successfully",
                Data = _mapper.Map<AdminData>(adminDetails)
            };

            return response;
        }

        public async Task<Response<AdminData>> GetAdminById(int adminId)
        {
            var adminDetails = await _unitOfWork.GetRepository<Employee>().Get(adminId);

            if (adminDetails == null || adminDetails.Role_Id != (int)RoleEnum.Admin)
            {
                throw new ApplicationException($"Admin with id - {adminId} not exists");
            }

            var response = new Response<AdminData>
            {
                Status = true,
                Message = $"Admin With Id - {adminId} Retreived Successfully",
                Data = _mapper.Map<AdminData>(adminDetails)
            };

            return response;
        }

        public async Task<Response<List<AdminData>>> GetAllAdmin()
        {
            var employeeList = await _unitOfWork.GetRepository<Employee>().GetAll();

            var adminData = employeeList.Where(x => x.Role_Id == (int)RoleEnum.Admin).ToList();

            if (adminData != null)
            {
                throw new ApplicationException($"Data not found");
            }

            var response = new Response<List<AdminData>>
            {
                Status = true,
                Message = $"Companies retrieved successfully",
                Data = _mapper.Map<List<AdminData>>(adminData.ToList())
            };

            return response;
        }

        public async Task<Response<AdminData>> UpdateAdmin(AdminData admin)
        {
            var adminData = await _unitOfWork.GetRepository<Employee>().Get(admin.Id);

            if (adminData == null || adminData.Role_Id != (int)RoleEnum.Admin)
            {
                throw new ApplicationException($"Admin with id - {admin.Id} not exists");
            }

            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            _mapper.Map(admin, adminData);
            adminData.UpdatedDate = DateTime.UtcNow;
            adminData.UpdatedBy = loggedInUser.EmployeeId;

            var result = _unitOfWork.GetRepository<Employee>().Update(adminData);
            await _unitOfWork.Save();

            var response = new Response<AdminData>
            {
                Status = true,
                Message = $"Company with id - {admin.Id} Updated Successfully",
                Data = admin
            };

            return response;
        }
    }
}

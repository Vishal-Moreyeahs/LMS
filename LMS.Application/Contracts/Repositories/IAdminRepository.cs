using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Application.Response;

namespace LMS.Application.Contracts.Repositories
{
    public interface IAdminRepository
    {
        Task<ApiResponse<RegistrationResponse>> AddAdmin(RegistrationRequest admin);

        Task<ApiResponse<AdminData>> DeleteAdmin(int adminId);
        Task<ApiResponse<AdminData>> UpdateAdmin(AdminData admin);
        Task<ApiResponse<List<AdminData>>> GetAllAdmin();

        Task<ApiResponse<AdminData>> GetAdminById(int admninId);
    }
}

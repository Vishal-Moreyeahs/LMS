using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Application.Response;
using LMS.Domain.Models;

namespace LMS.Application.Contracts.Repositories
{
    public interface IAdminRepository
    {
        Task<Response<RegistrationResponse>> AddAdmin(RegistrationRequest admin);

        Task<Response<AdminData>> DeleteAdmin(int adminId);
        Task<Response<AdminData>> UpdateAdmin(AdminData admin);
        Task<Response<List<AdminData>>> GetAllAdmin();

        Task<Response<AdminData>> GetAdminById(int admninId);
    }
}

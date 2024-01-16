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
    public interface IUserRepository
    {
        Task<Response<List<RegistrationResponse>>> AddUser(List<RegistrationRequest> users);

        Task<Response<UserData>> DeleteUser(int userId);
        Task<Response<UserData>> UpdateUser(UserData admin);
        Task<Response<List<UserData>>> GetAllUser();

        Task<Response<UserData>> GetUserById(int adminId);
    }
}

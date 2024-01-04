using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Application.Response;

namespace LMS.Application.Contracts.Infrastructure
{
    public interface IAuthService
    {
        Task<Response<AuthResponse>> Login(AuthRequest request);
        Task<Response<RegistrationResponse>> Register(RegistrationRequest request);

    }
}

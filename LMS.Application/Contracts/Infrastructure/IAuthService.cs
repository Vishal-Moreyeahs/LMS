using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Application.Response;
using LMS.Domain.Models;

namespace LMS.Application.Contracts.Infrastructure
{
    public interface IAuthService
    {
        Task<Response<AuthResponse>> Login(AuthRequest request);
        Task<Response<RegistrationResponse>> Register(RegistrationRequest request);
        Task<JwtSecurityToken> GenerateToken(Employee user);
        Task<Response<bool>> IsTokenValid(string jwtToken);

    }
}

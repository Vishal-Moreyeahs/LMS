using System.Security.Claims;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Response;
using Microsoft.AspNetCore.Http;

namespace LMS.Api.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthenticatedUserResponse> GetLoggedInUser()
        {
            var user = _httpContextAccessor.HttpContext?.User?.Identities.FirstOrDefault();

            if (user != null)
            {
                var companyIdClaim = user.FindFirst("CompanyId");
                var employeeIdClaim = user.FindFirst("EmployeeId");
                var roleIdClaim = user.FindFirst("RoleId");
                var emailClaim = user.FindFirst(ClaimTypes.Email);
                var nameClaim = user.FindFirst(ClaimTypes.Name);

                if (companyIdClaim != null && employeeIdClaim != null && roleIdClaim != null && emailClaim != null && nameClaim != null)
                {
                    return new AuthenticatedUserResponse
                    {
                        CompanyId = int.Parse(companyIdClaim.Value),
                        EmployeeId = int.Parse(employeeIdClaim.Value),
                        RoleId = int.Parse(roleIdClaim.Value),
                        Email = emailClaim.Value,
                        FirstName = nameClaim.Value
                    };
                }
            }

            return null;
        }
    }
}

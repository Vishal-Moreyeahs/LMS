using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using LMS.Application.Contracts.Infrastructure;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Application.Response;
using LMS.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LMS.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;
        private readonly ICryptographyService _cryptographyService;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public AuthService(IOptions<JwtSettings> jwtSettings, IMapper mapper, ICryptographyService cryptographyService
                        , IAuthenticatedUserService authenticatedUserService, IUnitOfWork unitOfWork)
        {
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
            _cryptographyService = cryptographyService;
            _authenticatedUserService = authenticatedUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<AuthResponse>> Login(AuthRequest request)
        {
            var user = _unitOfWork.GetRepository<Employee>().GetAll().Result.Where(x => x.Email == request.Email).FirstOrDefault();

            if (user == null)
            {
                throw new ApplicationException($"User with {request.Email} not found.");
            }
            var userEncryptedPassword = _cryptographyService.EncryptPassword(request.Email + request.Password);

            var isValid = _cryptographyService.ValidatePassword(user.Password, userEncryptedPassword);

            if (!isValid)
            {
                throw new ApplicationException($"Credentials for '{request.Email} aren't valid.");
            }

            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

            var authResponse = new AuthResponse
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email
            };
            var response = new Response<AuthResponse> {
                Status = true,
                Message = "Login Successfully",
                Data = authResponse
            };

            return response;
        }

        public async Task<Response<RegistrationResponse>> Register(RegistrationRequest request)
        {
            var existingUser = _unitOfWork.GetRepository<Employee>().GetAll().Result.Where(x => x.Email == request.Email).ToList();


            if (existingUser.Count > 0)
            {
                return null;
                //throw new ApplicationException($"User '{request.Email}' already exists.");
            }

            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

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

            var registrationResponse = new RegistrationResponse { UserId = registerUser.Id };
            var response = new Response<RegistrationResponse>
            {
                Status = true,
                Message = "Registered Successfully",
                Data = registrationResponse
            };
            return response;
        }

        private async Task<JwtSecurityToken> GenerateToken(Employee user)
        {
            
            var role = await _unitOfWork.GetRepository<Role>().Get(user.Role_Id);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, string.Concat(user.FirstName," ",user.LastName)),
                new Claim(CustomClaimTypes.CompanyId, user.Company_Id.ToString()),
                new Claim(CustomClaimTypes.RoleId, user.Role_Id.ToString()),
                new Claim(CustomClaimTypes.EmployeeId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role.Name)
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        public static class CustomClaimTypes
        {
            public const string CompanyId = "CompanyId";
            public const string RoleId = "RoleId";
            public const string EmployeeId = "EmployeeId";
        }

    }
}

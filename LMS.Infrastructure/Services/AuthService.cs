using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Application.Contracts.Infrastructure;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Application.Response;
using LMS.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LMS.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IGenericRepository<Employee> _employeeRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly ICryptographyService _cryptographyService;

        public AuthService(IGenericRepository<Role> roleRepository, 
                        IOptions<JwtSettings> jwtSettings, IGenericRepository<Employee> employeeRepository,
                        IGenericRepository<Company> companyRepository,IMapper mapper, ICryptographyService cryptographyService)
        {
            _roleRepository = roleRepository;
            _jwtSettings = jwtSettings.Value;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _cryptographyService = cryptographyService;
        }

        public async Task<AuthResponse> Login(AuthRequest request)
        {
            var user = _employeeRepository.GetAll().Result.Where(x => x.Email == request.Email).FirstOrDefault();

            if (user == null)
            {
                throw new Exception($"User with {request.Email} not found.");
            }
            var userEncryptedPassword = _cryptographyService.EncryptPassword(request.Email+request.Password);

            var result = _cryptographyService.ValidatePassword(user.Password, userEncryptedPassword);

            if (!result)
            {
                throw new Exception($"Credentials for '{request.Email} aren't valid'.");
            }

            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

            AuthResponse response = new AuthResponse
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email
            };

            return response;
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            var existingUser = _employeeRepository.GetAll().Result.Where(x => x.Email == request.Email).ToList();

            if (existingUser.Count > 0)
            {
                throw new Exception($"User '{request.Email}' already exists.");
            }

            var user = _mapper.Map<Employee>(request);
            user.CreatedDate = DateTime.UtcNow;
            user.UpdatedDate = DateTime.UtcNow;
            user.Password = _cryptographyService.EncryptPassword(request.Email+request.RealPassword);

            var registerUser = _employeeRepository.Add(user);

            if (registerUser.Result == null)
            {
                throw new Exception($"DataBase not Updated");
            }
            
            return new RegistrationResponse { UserId = registerUser.Result.Id };
        }

        private async Task<JwtSecurityToken> GenerateToken(Employee user)
        {
            //var userClaims = await _userManager.GetClaimsAsync(user);
            var role = _roleRepository.Get(user.Role_Id);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.FirstName),
                new Claim(JwtRegisteredClaimNames.Sub, user.LastName),
                new Claim("CompanyId", user.Company_Id.ToString()),
                new Claim("RoleId", user.Role_Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, role.Result.Name)
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
    }
}

using LMS.Application.Contracts.Infrastructure;
using LMS.Application.Request;
using LMS.Application.Response;
using LMS.Application.ViewModels;
using LMS.Domain.Models;
using LMS.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authenticationService;
        private readonly IUserManagerServices _userManagerServices;
        public AccountController(IAuthService authenticationService, IUserManagerServices userManagerServices)
        {
            _authenticationService = authenticationService;
            _userManagerServices = userManagerServices;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(AuthRequest request)
        {
            return Ok(await _authenticationService.Login(request));
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
        {
            return Ok(await _authenticationService.Register(request));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> Process(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            return Ok(_userManagerServices.ForgotPassword(forgotPasswordViewModel));
        }
    }
}

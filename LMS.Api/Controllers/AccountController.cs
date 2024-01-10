﻿using LMS.Application.Contracts.Infrastructure;
using LMS.Application.Request;
using LMS.Application.Response;
using LMS.Application.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Process(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            _userManagerServices.ForgotPassword(forgotPasswordViewModel);
            return Ok();
        }

        [HttpPost("verify-token")]
        public IActionResult VerifyResetToken(string token)
        {
            return Ok(_authenticationService.IsTokenValid(token));
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordViewModel token)
        {
            return Ok(_userManagerServices.ResetPassword(token));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await HttpContext.SignOutAsync();
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            }
            return Ok("Logout Successfully");
        }
    } 
}

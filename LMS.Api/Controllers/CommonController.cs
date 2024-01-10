using LMS.Application.Contracts.Repositories;
using LMS.Application.Request;
using LMS.Application.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public CommonController(IUserRepository userRepository)
        { 
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> CreateUser(List<RegistrationRequest> users)
        {
            return Ok(await _userRepository.AddUser(users));
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateUser(UserData user)
        {
            return Ok(await _userRepository.UpdateUser(user));
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            return Ok(await _userRepository.DeleteUser(id));
        }

        [HttpGet]
        [Route("getAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(await _userRepository.GetAllUser());
        }

        [HttpGet]
        [Route("getUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            return Ok(await _userRepository.GetUserById(id));
        }
    }
}

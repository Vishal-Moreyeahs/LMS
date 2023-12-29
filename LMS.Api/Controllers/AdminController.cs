using LMS.Application.Contracts.Repositories;
using LMS.Application.Request;
using LMS.Application.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        public AdminController(IAdminRepository adminRepository)
        { 
            _adminRepository = adminRepository;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> CreateAdmin(RegistrationRequest admin)
        {
            return Ok(await _adminRepository.AddAdmin(admin));
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateAdmin(AdminData admin)
        {
            return Ok(await _adminRepository.UpdateAdmin(admin));
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> DeleteAdminById(int adminId)
        {
            return Ok(await _adminRepository.DeleteAdmin(adminId));
        }

        [HttpPost]
        [Route("getAllAdmin")]
        public async Task<IActionResult> GetAllAdmin()
        {
            return Ok(await _adminRepository.GetAllAdmin());
        }

        [HttpPost]
        [Route("getAdminById")]
        public async Task<IActionResult> GetAdminById(int id)
        {
            return Ok(await _adminRepository.GetAdminById(id));
        }
    }
}

using LMS.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleServices _roleServices;
        public RoleController(IRoleServices roleServices)
        {
            _roleServices = roleServices;
        }

        [HttpGet]
        [Route("getRolesForLoggedInUser")]
        public async Task<IActionResult> GetRoles() 
        {
            return Ok(await _roleServices.GetRoleForLoggedInUser());
        }
    }
}

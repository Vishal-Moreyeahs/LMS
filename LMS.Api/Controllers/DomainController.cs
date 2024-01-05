using LMS.Application.Contracts.Repositories;
using LMS.Application.Request;
using LMS.Application.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LMS.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DomainController : ControllerBase
    {
        private readonly IDomainServices _domainRepository;

        public DomainController(IDomainServices domainRepository)
        {
            _domainRepository = domainRepository;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> CreateDomain(DomainRequest domain)
        {
            return Ok(await _domainRepository.AddDomain(domain));
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateDomain(DomainDTO domain)
        {
            return Ok(await _domainRepository.UpdateDomain(domain));
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> DeleteDomainById(int domainId)
        {
            return Ok(await _domainRepository.DeleteDomain(domainId));
        }

        [HttpPost]
        [Route("getAllDomain")]
        public async Task<IActionResult> GetAllDomain()
        {
            return Ok(await _domainRepository.GetAllDomain());
        }

        [HttpPost]
        [Route("getById")]
        public async Task<IActionResult> GetDomainById(int domainId)
        {
            return Ok(await _domainRepository.GetDomainById(domainId));
        }

        [HttpPost]
        [Route("addSubDomain")]
        public async Task<IActionResult> AddSubDomain(List<SubDomainRequest> subDomain)
        {
            return Ok(await _domainRepository.AddSubDomain(subDomain));
        }

        [HttpPost]
        [Route("updateSubDomain")]
        public async Task<IActionResult> UpdateSubDomain(SubDomainDTO subDomain)
        {
            return Ok(await _domainRepository.UpdateSubDomain(subDomain));
        }

        [HttpPost]
        [Route("deleteSubDomainWithId")]
        public async Task<IActionResult> DeleteSubDomainById(int subDomainId)
        {
            return Ok(await _domainRepository.DeleteSubDomain(subDomainId));
        }

        [HttpPost]
        [Route("getAllSubDomain")]
        public async Task<IActionResult> GetAllSubDomain()
        {
            return Ok(await _domainRepository.GetAllSubDomain());
        }

        [HttpPost]
        [Route("getSubDomainByDomainId")]
        public async Task<IActionResult> GetSubDomainByDomainId(int domainId)
        {
            return Ok(await _domainRepository.GetSubDomainByDomainId(domainId));
        }
    }
}

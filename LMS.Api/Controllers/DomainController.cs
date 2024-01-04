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
        private readonly IDomainRepository _domainRepository;
        private readonly ISubDomainRepository _subDomainRepository;

        public DomainController(IDomainRepository domainRepository, ISubDomainRepository subDomainRepository)
        {
            _domainRepository = domainRepository;
            _subDomainRepository = subDomainRepository;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> CreateCompany(DomainRequest domain)
        {
            return Ok(await _domainRepository.AddDomain(domain));
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateCompany(DomainDTO domain)
        {
            return Ok(await _domainRepository.UpdateDomain(domain));
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> DeleteCompanyById(int domainId)
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
        public async Task<IActionResult> AddSubDomain(SubDomainRequest subDomain)
        {
            return Ok(await _subDomainRepository.AddSubDomain(subDomain));
        }

        [HttpPost]
        [Route("updateSubDomain")]
        public async Task<IActionResult> UpdateSubDomain(SubDomainDTO subDomain)
        {
            return Ok(await _subDomainRepository.UpdateSubDomain(subDomain));
        }

        [HttpPost]
        [Route("deleteSubDomainWithId")]
        public async Task<IActionResult> DeleteSubDomainById(int subDomainId)
        {
            return Ok(await _subDomainRepository.DeleteSubDomain(subDomainId));
        }

        [HttpPost]
        [Route("getAllSubDomain")]
        public async Task<IActionResult> GetAllSubDomain()
        {
            return Ok(await _subDomainRepository.GetAllSubDomain());
        }

        [HttpPost]
        [Route("getSubDomainByDomainId")]
        public async Task<IActionResult> GetSubDomainByDomainId(int domainId)
        {
            return Ok(await _subDomainRepository.GetSubDomainByDomainId(domainId));
        }
    }
}

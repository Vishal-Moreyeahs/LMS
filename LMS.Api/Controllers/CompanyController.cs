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
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyController(ICompanyRepository companyRepository)
        { 
            _companyRepository = companyRepository;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCompany(CompanyRequest companyRequest)
        {
            return Ok(await _companyRepository.CreateCompany(companyRequest));
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateCompany(CompanyData companyRequest)
        {
            return Ok(await _companyRepository.UpdateCompany(companyRequest));
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteCompanyById(int companyId)
        {
            return Ok(await _companyRepository.DeleteCompany(companyId));
        }

        [HttpGet]
        [Route("getAllCompanies")]
        public async Task<IActionResult> GetAllCompany()
        {
            return Ok(await _companyRepository.GetAllCompany());
        }

        [HttpGet]
        [Route("getById")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            return Ok(await _companyRepository.GetCompanyById(id));
        }
    }
}

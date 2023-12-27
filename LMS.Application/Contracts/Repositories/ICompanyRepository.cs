using LMS.Application.Contracts.Persistence;
using LMS.Application.Request;
using LMS.Application.Response;
using LMS.Domain.Models;

namespace LMS.Application.Contracts.Repositories
{
    public interface ICompanyRepository
    {
        Task<CompanyResponse<CompanyData>> CreateCompany(CompanyRequest company);
        Task<CompanyResponse<CompanyData>> DeleteCompany(int companyId);
        Task<CompanyResponse<CompanyData>> UpdateCompany(CompanyData company);
        Task<CompanyResponse<List<CompanyData>>> GetAllCompany();

        Task<CompanyResponse<CompanyData>> GetCompanyById(int id);
    }
}

using AutoMapper;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Request;
using LMS.Application.Response;
using LMS.Domain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LMS.Application.Services.CompanyServices
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IGenericRepository<Company> _company;
        private IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public CompanyRepository(IGenericRepository<Company> company, IMapper mapper, IAuthenticatedUserService authenticatedUserService)
        {
            _company = company;
            _mapper = mapper;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<CompanyResponse<CompanyData>> CreateCompany(CompanyRequest company)
        {
            //Get all the companies
            var companyList = await _company.GetAll();

            //Check if company already exist in database
            bool isCompanyAlreadyExist = companyList.Any(x => x.PrimaryMailId == company.PrimaryMailId
                                               || x.OfficialMailId == company.OfficialMailId || x.Name.Contains(company.Name));

            if (isCompanyAlreadyExist)
            {
                return new CompanyResponse<CompanyData>
                {
                    Status = true,
                    Message = $"Company - {company.Name} Already Exist"
                };
            }

            var loggedInUser= await _authenticatedUserService.GetLoggedInUser();

            var data = _mapper.Map<Company>(company);
            data.CreatedDate = DateTime.UtcNow;
            data.UpdatedDate = DateTime.UtcNow;
            data.UpdatedBy = loggedInUser.EmployeeId;
            data.CreatedBy = loggedInUser.EmployeeId;

            var companyData = await _company.Add(data);

            if (companyData == null)
            {
                throw new Exception($"Error when adding Data in Database");
            }

            var response = new CompanyResponse<CompanyData> {
                Status = true,
                Message = $"Company - {company.Name} Added Successfully",
                Data = _mapper.Map<CompanyData>(companyData)
            };
            return response;
        }

        public async Task<CompanyResponse<CompanyData>> DeleteCompany(int companyId)
        {
            var companyDetails = await _company.Get(companyId);

            if (companyDetails == null)
            { 
                return new CompanyResponse<CompanyData> { 
                    Status= false,
                    Message = $"Company with id - {companyId} not exists"
                };
            }

            var result = _company.Delete(companyDetails);

            var response = new CompanyResponse<CompanyData>
            {
                Status = true,
                Message = $"Company With Id - {companyId} deleted Successfully",
                Data = _mapper.Map<CompanyData>(companyDetails)
            };

            return response;
        }

        public async Task<CompanyResponse<List<CompanyData>>> GetAllCompany()
        {
            var companyData = await _company.GetAll();

            if (companyData == null)
            { 
                return new CompanyResponse<List<CompanyData>>
                {
                    Status = true,
                    Message = $"No Data Found"
                };
            }

            var response = new CompanyResponse<List<CompanyData>>
            {
                Status = true,
                Message = $"Companies retrieved successfully",
                Data = _mapper.Map<List<CompanyData>>(companyData.ToList())
            };

            return response;
        }

        public async Task<CompanyResponse<CompanyData>> GetCompanyById(int companyId)
        {
            var companyDetails = await _company.Get(companyId);

            if (companyDetails == null)
            {
                return new CompanyResponse<CompanyData>
                {
                    Status = false,
                    Message = $"Company with id - {companyId} not exists"
                };
            }

            var response = new CompanyResponse<CompanyData>
            {
                Status = true,
                Message = $"Company Retreived Successfully",
                Data = _mapper.Map<CompanyData>(companyDetails)
            };

            return response;
        }

        public async Task<CompanyResponse<CompanyData>> UpdateCompany(CompanyData company)
        {
            var companyData = await _company.Get(company.Id);

            if (companyData == null)
            {
                return new CompanyResponse<CompanyData>
                {
                    Status = false,
                    Message = $"Company with id - {company.Id} not exists"
                };
            }

            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            _mapper.Map(company, companyData);
            companyData.UpdatedDate = DateTime.UtcNow;
            companyData.UpdatedBy = loggedInUser.EmployeeId;

            var result = _company.Update(companyData);

            var response = new CompanyResponse<CompanyData>
            {
                Status = true,
                Message = $"Company with id - {company.Id} Updated Successfully",
                Data = company
            };

            return response;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Application.Response;
using LMS.Domain.Enums;
using LMS.Domain.Models;

namespace LMS.Application.Services.DomainServices
{
    public class DomainRepository : IDomainRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public DomainRepository(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUserService,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUserService = authenticatedUserService;
            _mapper = mapper;
        }

        public async Task<Response<DomainRequest>> AddDomain(DomainRequest domain)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            if (loggedInUser == null || loggedInUser.RoleId != (int)RoleEnum.Admin)
            {
                throw new ApplicationException($"Logged in user does not have Permission to Add Domain");
            }

            var domains = await _unitOfWork.GetRepository<Domains>().GetAll();
            var isDomainExist = domains.Any(x => x.Name == domain.Name && x.Company_Id == loggedInUser.CompanyId);

            if (isDomainExist)
            {
                throw new ApplicationException($"Domain Name - {domain.Name} already exist");
            }

            var data = _mapper.Map<Domains>(domain);
            data.CreatedDate = DateTime.UtcNow;
            data.UpdatedDate = DateTime.UtcNow;
            data.UpdatedBy = loggedInUser.EmployeeId;
            data.CreatedBy = loggedInUser.EmployeeId;
            data.Company_Id = loggedInUser.CompanyId;

            var res = await _unitOfWork.GetRepository<Domains>().Add(data);
            var isDataAdded = await _unitOfWork.Save();
            if (isDataAdded <= 0)
            {
                throw new ApplicationException($"domain should not be added");
            }
            var response = new Response<DomainRequest>
            { 
                Status = true,
                Message = $"Domain - {domain.Name} Added Successfully",
                Data = domain
            };
            return response;
        }

        public async Task<Response<DomainDTO>> DeleteDomain(int domainId)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            var domain = await _unitOfWork.GetRepository<Domains>().Get(domainId);

            if (domain == null || domain.Company_Id != loggedInUser.CompanyId)
            {
                throw new ApplicationException($"Domain with Id - {domainId} not exist");
            }

            domain.IsActive = true;
            var result = _unitOfWork.GetRepository<Domains>().Update(domain);
            await _unitOfWork.Save();
            var isDomainDeleted = await _unitOfWork.Save();
            if (isDomainDeleted <= 0)
            {
                throw new ApplicationException($"Domain with Id - {domainId} should not delete");
            }
            var data = _mapper.Map<DomainDTO>(domain);
            var response = new Response<DomainDTO>
            {
                Status= true,
                Message = $"Domain with Id - {domainId} deleted Successfully",
                Data= data
            };
            return response;
        }

        public async Task<Response<List<DomainDTO>>> GetAllDomain()
        {
            var allDomains = await _unitOfWork.GetRepository<Domains>().GetAll();

            if (allDomains == null)
            {
                throw new ApplicationException($"No Data Found");
            }

            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            var domains = allDomains.Where(x => x.Company_Id == loggedInUser.CompanyId && x.IsActive);
            var response = new Response<List<DomainDTO>>
            {
                Status = true,
                Message = $"Domain retrieved successfully",
                Data = _mapper.Map<List<DomainDTO>>(domains.ToList())
            };

            return response;
        }

        public async Task<Response<DomainDTO>> GetDomainById(int domainId)
        {

            var domainDetail = await _unitOfWork.GetRepository<Domains>().Get(domainId);
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            if (domainDetail == null || domainDetail.Company_Id != loggedInUser.CompanyId || !domainDetail.IsActive)
            {
                throw new ApplicationException($"Domain with id - {domainId} not exists");
            }
            
            var response = new Response<DomainDTO>
            {
                Status = true,
                Message = $"Domain With Id - {domainId} Retreived Successfully",
                Data = _mapper.Map<DomainDTO>(domainDetail)
            };

            return response;
        }

        public async Task<Response<DomainDTO>> UpdateDomain(DomainDTO domain)
        {
            var domainData = await _unitOfWork.GetRepository<Domains>().Get(domain.Id);
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            if (domainData == null || domainData.Company_Id != loggedInUser.CompanyId)
            {
                throw new ApplicationException($"Domain with id - {domain.Id} not exists");
            }

            _mapper.Map(domain, domainData);
            domainData.UpdatedDate = DateTime.UtcNow;
            domainData.UpdatedBy = loggedInUser.EmployeeId;

            var result = _unitOfWork.GetRepository<Domains>().Update(domainData);
            var isDomainUpdated = await _unitOfWork.Save();

            if (isDomainUpdated <= 0)
            {
                throw new ApplicationException($"Domain with Id - {domain.Id} should not Update");
            }

            var response = new Response<DomainDTO>
            {
                Status = true,
                Message = $"Company with id - {domain.Id} Updated Successfully",
                Data = domain
            };

            return response;
        }
    }
}

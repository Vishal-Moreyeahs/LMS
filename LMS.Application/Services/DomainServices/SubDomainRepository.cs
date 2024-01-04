using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Domain.Enums;
using LMS.Domain.Models;

namespace LMS.Application.Services.DomainServices
{
    public class SubDomainRepository : ISubDomainRepository
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDomainRepository _domainRepository;
        public SubDomainRepository(IAuthenticatedUserService authenticatedUserService, IUnitOfWork unitOfWork, IMapper mapper, IDomainRepository domainRepository)
        {
            _authenticatedUserService = authenticatedUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _domainRepository = domainRepository;
        }

        public async Task<Response<SubDomainDTO>> AddSubDomain(SubDomainRequest subDomain)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            if (loggedInUser == null || loggedInUser.RoleId != (int)RoleEnum.Admin)
            {
                throw new ApplicationException($"Logged in user does not have Permission to Add Sub-Domain");
            }

            var domain = await _domainRepository.GetDomainById(subDomain.Domain_Id);

            if (domain == null)
            {
                throw new ApplicationException($"Domain Id - {subDomain.Domain_Id} not exist");
            }

            var data = _mapper.Map<SubDomain>(subDomain);
            data.CreatedDate = DateTime.UtcNow;
            data.UpdatedDate = DateTime.UtcNow;
            data.UpdatedBy = loggedInUser.EmployeeId;
            data.CreatedBy = loggedInUser.EmployeeId;

            var res = await _unitOfWork.GetRepository<SubDomain>().Add(data);
            var isDataAdded = await _unitOfWork.Save();
            if (isDataAdded <= 0)
            {
                throw new ApplicationException($"Sub Domain should not be added");
            }
            var response = new Response<SubDomainDTO>
            {
                Status = true,
                Message = $"Domain - {domain} Added Successfully",
                Data = _mapper.Map<SubDomainDTO>(data)
            };
            return response;
        }

        public async Task<Response<SubDomainDTO>> DeleteSubDomain(int subDomainId)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            var subDomain = await _unitOfWork.GetRepository<SubDomain>().Get(subDomainId);
            var domain = await _unitOfWork.GetRepository<Domains>().Get(subDomain.Domain_Id);
            if (domain == null || domain.Company_Id != loggedInUser.CompanyId || subDomain == null)
            {
                throw new ApplicationException($"Sub Domain with Id - {subDomainId} not exist");
            }

            domain.IsActive = true;
            var result = _unitOfWork.GetRepository<Domains>().Update(domain);
            await _unitOfWork.Save();
            var isSubDomainDeleted = await _unitOfWork.Save();
            if (isSubDomainDeleted <= 0)
            {
                throw new ApplicationException($"Sub Domain with Id - {subDomainId} should not delete");
            }
            var data = _mapper.Map<SubDomainDTO>(domain);
            var response = new Response<SubDomainDTO>
            {
                Status = true,
                Message = $"Sub Domain with Id - {subDomainId} deleted Successfully",
                Data = data
            };
            return response;
        }

        public async Task<Response<List<SubDomainDTO>>> GetAllSubDomain()
        {
            var allSubDomains = await _unitOfWork.GetRepository<SubDomain>().GetAll();
            var allDomains = await _unitOfWork.GetRepository<Domains>().GetAll();
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            var subDomains = from subDomain in allSubDomains
                             join dmn in allDomains 
                             on subDomain.Domain_Id equals dmn.Id
                             where dmn.Company_Id == loggedInUser.CompanyId && subDomain.IsActive
                             select new SubDomainDTO
                             { 
                                 Domain_Id = dmn.Id,
                                 Description = subDomain.Description,
                                 Id = subDomain.Id,
                                 Name = subDomain.Name,
                             };

            var response = new Response<List<SubDomainDTO>>
            {
                Status = true,
                Message = $"Sub Domain retrieved successfully",
                Data = _mapper.Map<List<SubDomainDTO>>(subDomains.ToList())
            };

            return response;
        }

        public async Task<Response<List<SubDomainDTO>>> GetSubDomainByDomainId(int domainId)
        {
            var allSubDomains = await _unitOfWork.GetRepository<SubDomain>().GetAll();
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            var domain = await _unitOfWork.GetRepository<Domains>().Get(domainId);
            if (domain == null || domain.Company_Id != loggedInUser.CompanyId)
            {
                throw new ApplicationException($"Domain Id - {domainId} not exist");
            }
            var domainRelatedSubDomains = allSubDomains.Where(x => x.Domain_Id == domainId && x.IsActive).ToList();

            var response = new Response<List<SubDomainDTO>>
            {
                Status = true,
                Message = $"Sub Domain retrieved successfully",
                Data = _mapper.Map<List<SubDomainDTO>>(domainRelatedSubDomains.ToList())
            };

            return response;
        }

        public async Task<Response<SubDomainDTO>> UpdateSubDomain(SubDomainDTO subDomain)
        {
            var subDomainData = await _unitOfWork.GetRepository<SubDomain>().Get(subDomain.Id);
            if (subDomainData == null)
            {
                throw new ApplicationException($"SubDomain with id - {subDomain.Id} not exists");
            }
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            var domainData = await _unitOfWork.GetRepository<Domains>().Get(subDomainData.Domain_Id);

            if (domainData == null || domainData.Company_Id != loggedInUser.CompanyId)
            {
                throw new ApplicationException($"SubDomain with id - {subDomain.Id} not exists");
            }

            _mapper.Map(subDomain, subDomainData);
            subDomainData.UpdatedDate = DateTime.UtcNow;
            subDomainData.UpdatedBy = loggedInUser.EmployeeId;

            var result = _unitOfWork.GetRepository<SubDomain>().Update(subDomainData);
            var isSubDomainUpdated = await _unitOfWork.Save();

            if (isSubDomainUpdated <= 0)
            {
                throw new ApplicationException($"Sub Domain with Id - {subDomain.Id} should not Update");
            }

            var response = new Response<SubDomainDTO>
            {
                Status = true,
                Message = $"SubDomain with id - {subDomain.Id} Updated Successfully",
                Data = subDomain
            };

            return response;
        }
    }
}

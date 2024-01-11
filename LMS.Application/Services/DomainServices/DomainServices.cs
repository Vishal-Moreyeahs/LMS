﻿using System;
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
    public class DomainServices : IDomainServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public DomainServices(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUserService,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUserService = authenticatedUserService;
            _mapper = mapper;
        }

        public async Task<Response<DomainDTO>> AddDomain(DomainRequest domain)
        {
            if (domain.SubDomains == null || domain.SubDomains.Count <= 0)
            {
                throw new ApplicationException($"Request does not contain SubDomain !!");
            }
            var domainData = await CreateDomain(domain);
            var response = new Response<DomainDTO>
            {
                Status = true,
                Message = $"Domain with related SubDomain Added successfully."
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

            var subDomains = await _unitOfWork.GetRepository<SubDomain>().GetAll();
            
            domainDetail.SubDomains = subDomains.Where(x => x.IsActive && x.Domain_Id == domainDetail.Id).ToList();

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

        public async Task CreateSubDomain(SubDomainRequest subDomain)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            if (loggedInUser == null || loggedInUser.RoleId != (int)RoleEnum.Admin)
            {
                throw new ApplicationException($"Logged in user does not have Permission to Add Sub-Domain");
            }

            var domain = await GetDomainById(subDomain.Domain_Id);
            
            if (domain == null)
            {
                throw new ApplicationException($"Domain Id - {subDomain.Domain_Id} not exist");
            }

            var subDomains = await _unitOfWork.GetRepository<SubDomain>().GetAll();
            var isSubDomainExist = subDomains.Any(x => x.Name == subDomain.Name && x.IsActive);
            if (isSubDomainExist)
            {
                return;
            }

            var data = _mapper.Map<SubDomain>(subDomain);
            data.CreatedDate = DateTime.UtcNow;
            data.UpdatedDate = DateTime.UtcNow;
            data.UpdatedBy = loggedInUser.EmployeeId;
            data.CreatedBy = loggedInUser.EmployeeId;

             await _unitOfWork.GetRepository<SubDomain>().Add(data);
            var isDataAdded = await _unitOfWork.Save();
            if (isDataAdded <= 0)
            {
                throw new ApplicationException($"Sub Domain should not be added");
            }
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

        private async Task<DomainDTO> CreateDomain(DomainRequest domain)
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
            if(data.SubDomains != null)
            {
                foreach (var subdomain in data.SubDomains)
                {
                    subdomain.CreatedBy = loggedInUser.EmployeeId;
                    subdomain.UpdatedBy = loggedInUser.EmployeeId;
                }
            }

            await _unitOfWork.GetRepository<Domains>().Add(data);
            var isDataAdded = await _unitOfWork.Save();
            if (isDataAdded <= 0)
            {
                throw new ApplicationException($"domain should not be added");
            }
            var response = _mapper.Map<DomainDTO>(data);
            return response;
        }

        public async Task<Response<SubDomainDTO>> AddSubDomain(List<SubDomainRequest> subDomains)
        {
            if (subDomains == null || subDomains.Count <= 0)
            {
                throw new ApplicationException($"SubDomain is empty !!");
            }
            foreach (var item in subDomains)
            { 
                await CreateSubDomain(item);
            }
            var response = new Response<SubDomainDTO>
            {
                Status = true,
                Message = $"Domain with related SubDomain Added successfully."
            };
            return response;
        }
    }
}

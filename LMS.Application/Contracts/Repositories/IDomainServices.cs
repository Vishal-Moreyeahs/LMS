using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Application.Response;

namespace LMS.Application.Contracts.Repositories
{
    public interface IDomainServices
    {
        Task<Response<DomainDTO>> AddDomain(DomainRequest admin);

        Task<Response<DomainDTO>> DeleteDomain(int domainId);
        Task<Response<DomainDTO>> UpdateDomain(DomainDTO admin);
        Task<Response<List<DomainDTO>>> GetAllDomain();

        Task<Response<DomainDTO>> GetDomainById(int domainId);
        Task<Response<SubDomainDTO>> AddSubDomain(List<SubDomainRequest> subDomain);
        Task<Response<SubDomainDTO>> DeleteSubDomain(int domainId);
        Task<Response<SubDomainDTO>> UpdateSubDomain(SubDomainDTO admin);
        Task<Response<List<SubDomainDTO>>> GetAllSubDomain();
        Task<Response<List<SubDomainDTO>>> GetSubDomainByDomainId(int domainId);
    }
}

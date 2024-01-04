using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Models;
using LMS.Application.Request;

namespace LMS.Application.Contracts.Repositories
{
    public interface ISubDomainRepository
    {
        Task<Response<SubDomainDTO>> AddSubDomain(SubDomainRequest subDomain);
        Task<Response<SubDomainDTO>> DeleteSubDomain(int domainId);
        Task<Response<SubDomainDTO>> UpdateSubDomain(SubDomainDTO admin);
        Task<Response<List<SubDomainDTO>>> GetAllSubDomain();
        Task<Response<List<SubDomainDTO>>> GetSubDomainByDomainId(int domainId);
    }
}

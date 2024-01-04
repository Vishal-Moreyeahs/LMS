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
    public interface IDomainRepository
    {
        Task<Response<DomainRequest>> AddDomain(DomainRequest admin);

        Task<Response<DomainDTO>> DeleteDomain(int domainId);
        Task<Response<DomainDTO>> UpdateDomain(DomainDTO admin);
        Task<Response<List<DomainDTO>>> GetAllDomain();

        Task<Response<DomainDTO>> GetDomainById(int domainId);
    }
}

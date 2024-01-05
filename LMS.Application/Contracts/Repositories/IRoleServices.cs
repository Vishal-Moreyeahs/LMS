using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Models;
using LMS.Domain.Models;

namespace LMS.Application.Contracts.Repositories
{
    public interface IRoleServices
    {
        Task<Response<List<Role>>> GetRoleForLoggedInUser();
    }
}
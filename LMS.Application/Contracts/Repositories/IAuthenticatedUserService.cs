using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Response;

namespace LMS.Application.Contracts.Repositories
{
    public interface IAuthenticatedUserService
    {
       Task<AuthenticatedUserResponse> GetLoggedInUser();
    }
}

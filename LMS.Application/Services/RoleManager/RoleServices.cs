using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Models;
using LMS.Domain.Enums;
using LMS.Domain.Models;

namespace LMS.Application.Services.RoleManager
{
    public class RoleServices : IRoleServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public RoleServices(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUserService)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<Response<List<Role>>> GetAllRole()
        {
            var roleList = await _unitOfWork.GetRepository<Role>().GetAll();

            var response = new Response<List<Role>> { 
                Status = true,
                Message = "Roles Retreived Successfully",
                Data = roleList.ToList()
            };

            return response;
        }

        public async Task<Response<List<Role>>> GetRoleForLoggedInUser()
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            var roleList = await _unitOfWork.GetRepository<Role>().GetAll();

            var response = new Response<List<Role>>
            {
                Status = true,
                Message = "Roles Retreived Successfully"
            };

            if (loggedInUser.RoleId == (int)RoleEnum.SuperAdmin)
            {
                roleList = roleList.Where(x => x.Id != (int)RoleEnum.SuperAdmin).ToList();
            }
            if (loggedInUser.RoleId == (int)RoleEnum.User)
            {
                roleList = roleList.Where(x => x.Id != (int)RoleEnum.SuperAdmin && x.Id != (int)RoleEnum.Admin && x.Id != (int)RoleEnum.User).ToList();
            }
            if (loggedInUser.RoleId == (int)RoleEnum.Guest)
            {
                roleList = null;
            }
            response.Data = roleList.ToList();
            return response;
        }
    }
}

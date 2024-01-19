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
using LMS.Domain.Models;

namespace LMS.Application.Services.EmployeeQuizManager
{
    public class EmployeeQuizServices : IEmployeeQuizServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public EmployeeQuizServices(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<dynamic>> AssignQuizToEmployees(List<EmployeeQuizRequest> employeeQuizRequests)
        {
            if (employeeQuizRequests.Count == 0 && employeeQuizRequests == null)
            {
                throw new ApplicationException("Invalid Request");
            }

            var data = _mapper.Map<List<EmployeeQuiz>>(employeeQuizRequests);

            await _unitOfWork.GetRepository<EmployeeQuiz>().AddRange(data);
            var isDataAdded = await _unitOfWork.SaveChangesAsync();
            if (isDataAdded <= 0)
            {
                throw new ApplicationException("Unable to assign Quiz to employees");
            }
            var response = new Response<dynamic>
            {
                Status = true,
                Message = "Successfully added the Quiz to employees"
            };
            return response;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Application.Contracts.Infrastructure;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Domain.Enums;
using LMS.Domain.Models;

namespace LMS.Application.Services.CourseManager
{
    public class CourseServices : ICourseServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public CourseServices(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUserService,
                            IMapper mapper)
        { 
            _unitOfWork = unitOfWork;
            _authenticatedUserService = authenticatedUserService;
            _mapper = mapper;
        }

        public async Task<Response<CourseRequest>> AddCourse(CourseRequest course)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            
            var courseData = _mapper.Map<Course>(course);
            courseData.Company_Id = loggedInUser.CompanyId;
            courseData.CreatedDate = DateTime.UtcNow;
            courseData.UpdatedDate = DateTime.UtcNow;
            courseData.CreatedBy = loggedInUser.EmployeeId;
            courseData.UpdatedBy = loggedInUser.EmployeeId;

            await _unitOfWork.GetRepository<Course>().Add(courseData);
            var isDataAdded = await _unitOfWork.Save();
            if (isDataAdded <= 0)
            {
                throw new ApplicationException($"Fail to Add course data");
            }
            var response = new Response<CourseRequest>
            { 
                Status = true,
                Message = "",
                Data = _mapper.Map<CourseRequest>(course)
            };
            return null;
        }
    }
}
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
            if (course.CourseContents == null || course.CourseContents.Count <= 0)
            {
                throw new ApplicationException($"Request does not contain CourseContent !!");
            }
            var courseData = await CreateCourse(course);
            var response = new Response<CourseRequest>
            {
                Status = true,
                Message = $"Course with related CourseContent Added successfully."
            };
            return response;
        }

        public async Task<Response<List<string>>> GetAllContentType()
        {
            var contentTypeList = Enum.GetNames(typeof(MediaFormat)).ToList();

            var response = new Response<List<string>>
            {
                Status = true,
                Message = "Content Types Retreived",
                Data = contentTypeList
            };
            return response;
        }

        public async Task<Response<List<CourseDTO>>> GetAllCourse()
        {
            var allCourses = await _unitOfWork.GetRepository<Course>().GetAll();

            if (allCourses == null)
            {
                throw new ApplicationException($"No Data Found");
            }

            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            var courses = allCourses.Where(x => x.Company_Id == loggedInUser.CompanyId && x.IsActive).ToList();
            var response = new Response<List<CourseDTO>>
            {
                Status = true,
                Message = $"Domain retrieved successfully",
                Data = _mapper.Map<List<CourseDTO>>(courses.ToList())
            };

            return response;
        }

        public async Task<Response<CourseRequest>> GetCourseById(int id)
        {
            var courseDetail = await _unitOfWork.GetRepository<Course>().Get(id);
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            if (courseDetail == null || courseDetail.Company_Id != loggedInUser.CompanyId || !courseDetail.IsActive)
            {
                throw new ApplicationException($"Domain with id - {id} not exists");
            }

            var courseContents = await _unitOfWork.GetRepository<CourseContent>().GetAll();

            courseDetail.CourseContents = courseContents.Where(x => x.IsActive && x.Courses_Id == courseDetail.Id).OrderBy(x => x.Sequence).ToList();

            var response = new Response<CourseRequest>
            {
                Status = true,
                Message = $"Domain With Id - {id} Retreived Successfully",
                Data = _mapper.Map<CourseRequest>(courseDetail)
            };

            return response;
        }

        private async Task<CourseRequest> CreateCourse(CourseRequest course)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            if (loggedInUser == null || loggedInUser.RoleId != (int)RoleEnum.Admin)
            {
                throw new ApplicationException($"Logged in user does not have Permission to Add Course");
            }

            var courses = await _unitOfWork.GetRepository<Course>().GetAll();
            var isDomainExist = courses.Any(x => x.Name == course.Name && x.Company_Id == loggedInUser.CompanyId);

            if (isDomainExist)
            {
                throw new ApplicationException($"Course Name - {course.Name} already exist");
            }

            var data = _mapper.Map<Course>(course);
            data.CreatedDate = DateTime.UtcNow;
            data.UpdatedDate = DateTime.UtcNow;
            data.UpdatedBy = loggedInUser.EmployeeId;
            data.CreatedBy = loggedInUser.EmployeeId;
            data.Company_Id = loggedInUser.CompanyId;
            if (data.CourseContents != null)
            {
                foreach (var courseContent in data.CourseContents)
                {
                    courseContent.CreatedBy = loggedInUser.EmployeeId;
                    courseContent.UpdatedBy = loggedInUser.EmployeeId;
                }
            }

            await _unitOfWork.GetRepository<Course>().Add(data);
            var isDataAdded = await _unitOfWork.Save();
            if (isDataAdded <= 0)
            {
                throw new ApplicationException($"domain should not be added");
            }
            var response = _mapper.Map<CourseRequest>(data);
            return response;
        }
    }
}
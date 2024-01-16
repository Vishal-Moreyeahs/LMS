using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Application.Contracts.Infrastructure;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Application.Response;
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

        public async Task<Response<CourseDTO>> DeleteCourse(int id)
        {
            var courseDetails = await _unitOfWork.GetRepository<Course>().Get(id);

            if (courseDetails == null || !courseDetails.IsActive)
            {
                throw new ApplicationException($"Course with id - {id} not exists");
            }

            courseDetails.IsActive = false;
            await _unitOfWork.GetRepository<Course>().Update(courseDetails);
            await _unitOfWork.SaveChangesAsync();

            var response = new Response<CourseDTO>
            {
                Status = true,
                Message = $"Course With Id - {id} deleted Successfully"
            };

            return response;
        }

        public async Task<Response<CourseContentDto>> DeleteCourseContent(int id)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            var courseContent = await _unitOfWork.GetRepository<CourseContent>().Get(id);
            var course = await _unitOfWork.GetRepository<Course>().Get(courseContent.Courses_Id);
            if (course == null || course.Company_Id != loggedInUser.CompanyId || courseContent == null)
            {
                throw new ApplicationException($"Course Content with Id - {id} not exist");
            }

            courseContent.IsActive = false;
            await _unitOfWork.GetRepository<CourseContent>().Update(courseContent);
            await _unitOfWork.SaveChangesAsync();
            var isCourseContentAdded = await _unitOfWork.SaveChangesAsync();
            if (isCourseContentAdded <= 0)
            {
                throw new ApplicationException($"Course Content with Id - {id} should not delete");
            }
            var data = _mapper.Map<CourseContentDto>(courseContent);
            var response = new Response<CourseContentDto>
            {
                Status = true,
                Message = $"Course content with Id - {id} deleted Successfully",
                Data = data
            };
            return response;
        }

        public async Task<Response<CourseDTO>> UpdateCourse(CourseDTO course)
        {
            var courseDetails = await _unitOfWork.GetRepository<Course>().Get(course.Id);
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            if (courseDetails == null || !courseDetails.IsActive)
            {
                throw new ApplicationException($"Course with id - {course.Id} does not exist");
            }

            _mapper.Map(course, courseDetails);
            //courseDetails.UpdatedDate = DateTime.UtcNow;
            //courseDetails.UpdatedBy = loggedInUser.EmployeeId;

            await _unitOfWork.GetRepository<Course>().Update(courseDetails);
            var isCourseUpdate = await _unitOfWork.SaveChangesAsync();

            if (isCourseUpdate <= 0)
            {
                throw new ApplicationException($"Course with Id - {course.Id} should not update");
            }

            var response = new Response<CourseDTO>
            {
                Status = true,
                Message = $"Course with id - {course.Id} Updated Successfully",
                Data = course
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
            //data.CreatedDate = DateTime.UtcNow;
            //data.UpdatedDate = DateTime.UtcNow;
            //data.UpdatedBy = loggedInUser.EmployeeId;
            //data.CreatedBy = loggedInUser.EmployeeId;
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
            var isDataAdded = await _unitOfWork.SaveChangesAsync();
            if (isDataAdded <= 0)
            {
                throw new ApplicationException($"domain should not be added");
            }
            var response = _mapper.Map<CourseRequest>(data);
            return response;
        }

        public async Task<Response<CourseContentDto>> AddCourseContent(List<CourseContentRequest> courseContents)
        {
            if (courseContents == null || courseContents.Count <= 0)
            {
                throw new ApplicationException($"Course Content is empty !!");
            }
            foreach (var item in courseContents)
            {
                await CreateCourseContent(item);
            }
            var response = new Response<CourseContentDto>
            {
                Status = true,
                Message = $"Course content added for particular course."
            };
            return response;
        }

        public async Task CreateCourseContent(CourseContentRequest courseContent)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            if (loggedInUser == null || loggedInUser.RoleId != (int)RoleEnum.Admin)
            {
                throw new ApplicationException($"Logged in user does not have Permission to Add course content");
            }

            var course = await GetCourseById((int)courseContent.Courses_Id);

            if (course == null)
            {
                throw new ApplicationException($"course Id - {courseContent.Courses_Id} not exist");
            }

            var courseContents = await _unitOfWork.GetRepository<CourseContent>().GetAll();
            var isCourseContentsExist = courseContents.Any(x => x.Title == courseContent.Title && x.IsActive);
            if (isCourseContentsExist)
            {
                return;
            }

            var data = _mapper.Map<CourseContent>(courseContents);
            //data.CreatedDate = DateTime.UtcNow;
            //data.UpdatedDate = DateTime.UtcNow;
            //data.UpdatedBy = loggedInUser.EmployeeId;
            //data.CreatedBy = loggedInUser.EmployeeId;

            await _unitOfWork.GetRepository<CourseContent>().Add(data);
            var isDataAdded = await _unitOfWork.SaveChangesAsync();
            if (isDataAdded <= 0)
            {
                throw new ApplicationException($"Sub Domain should not be added");
            }
        }

        public async Task<Response<CourseContentDto>> GetCourseContentById(int id)
        {
            var courseContent = await _unitOfWork.GetRepository<CourseContent>().Get(id);
            var course = await _unitOfWork.GetRepository<Course>().Get(courseContent.Id);
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            if (courseContent == null || loggedInUser.CompanyId == course.Company_Id || course == null)
            {
                throw new ApplicationException($"Course content with id - {id} does not exist");
            }
            var response = new Response<CourseContentDto>()
            { 
                Status = true,
                Message = $"Course content retreived",
                Data = _mapper.Map<CourseContentDto>(courseContent)
            };
            return response;
        }

        public async Task<Response<List<CourseContentDto>>> GetAllCourseContentByCourseId(int courseId)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            var course = await _unitOfWork.GetRepository<Course>().Get(courseId);
            if (course == null || course.Company_Id == loggedInUser.CompanyId)
            {
                throw new ApplicationException($"Course with course id - {courseId} not exists");
            }
            var allCourseContents = await _unitOfWork.GetRepository<CourseContent>().GetAll();

            var courseContent = allCourseContents.Where(x => x.IsActive && x.Courses_Id == courseId).ToList();

            var response = new Response<List<CourseContentDto>>
            { 
                Status = true,
                Message = $"",
                Data = _mapper.Map<List<CourseContentDto>>(courseContent)
            };
            return response;
        }

        public async Task<Response<CourseContentDto>> UpdateCourseContent(CourseContentDto courseContentDto)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            var courseContentDetails = await _unitOfWork.GetRepository<CourseContent>().Get(courseContentDto.Id);

            if (courseContentDetails == null)
            {
                throw new ApplicationException($"Course content does not exists");
            }

            _mapper.Map<CourseContent>(courseContentDto);
            _mapper.Map(courseContentDto, courseContentDetails);
            //courseContentDetails.UpdatedDate = DateTime.UtcNow;
            //courseContentDetails.UpdatedBy = loggedInUser.EmployeeId;

            await _unitOfWork.GetRepository<CourseContent>().Update(courseContentDetails);
            var isCourseContentUpdate = await _unitOfWork.SaveChangesAsync();

            if (isCourseContentUpdate <= 0)
            {
                throw new ApplicationException($"Course content with Id - {courseContentDto.Id} should not update");
            }

            var response = new Response<CourseContentDto>
            {
                Status = true,
                Message = $"Course with id - {courseContentDto.Id} Updated Successfully",
                Data = _mapper.Map<CourseContentDto>(courseContentDetails)
            };

            return response;
        }
    }
}
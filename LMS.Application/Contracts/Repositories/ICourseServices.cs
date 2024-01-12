using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Application.Response;
using LMS.Domain.Models;

namespace LMS.Application.Contracts.Repositories
{
    public interface ICourseServices
    {
        Task<Response<CourseRequest>> AddCourse(CourseRequest course);
        Task<Response<CourseDTO>> UpdateCourse(CourseDTO course);
        Task<Response<List<CourseDTO>>> GetAllCourse();
        Task<Response<CourseRequest>> GetCourseById(int id);
        Task<Response<List<string>>> GetAllContentType();
        Task<Response<CourseDTO>> DeleteCourse(int id);
        Task<Response<CourseContentDto>> AddCourseContent(List<CourseContentRequest> courseContents);
        Task<Response<CourseContentDto>> DeleteCourseContent(int id);
        Task<Response<CourseContentDto>> GetCourseContentById(int id);
        Task<Response<List<CourseContentDto>>> GetAllCourseContentByCourseId(int courseId);
        Task<Response<CourseContentDto>> UpdateCourseContent(CourseContentDto courseContentDto);
    }
}

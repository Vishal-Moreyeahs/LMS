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
        Task<Response<CourseDTO>> UpdateCompany(CourseDTO course);
        Task<Response<List<CourseDTO>>> GetAllCourse();
        Task<Response<CourseRequest>> GetCourseById(int id);
        Task<Response<List<string>>> GetAllContentType();
        Task<Response<CourseDTO>> DeleteCourse(int id);
    }
}

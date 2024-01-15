using LMS.Application.Models;
using LMS.Application.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Contracts.Repositories
{
    public interface IQuizServices
    {
        Task<Response<dynamic>> GetAllQuizes();
        Task<Response<dynamic>> GetQuizById(int id);
        Task<Response<dynamic>> CreateQuiz(QuizRequest quiz);
    }
}

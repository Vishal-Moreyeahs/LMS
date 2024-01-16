using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Models;

namespace LMS.Application.Contracts.Repositories
{
    public interface IQuizQuestionServices
    {
        Task<Response<dynamic>> GetQuizQuestions(int quizId);
        Task<Response<dynamic>> AssignQuestionToQuiz();
        Task<Response<dynamic>> DeleteQuestionFromQuiz();
    }
}

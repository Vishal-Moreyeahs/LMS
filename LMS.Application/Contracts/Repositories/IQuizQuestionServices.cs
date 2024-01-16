using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Models;
using LMS.Application.Request;

namespace LMS.Application.Contracts.Repositories
{
    public interface IQuizQuestionServices
    {
        Task<Response<dynamic>> GetQuizQuestions(int quizId);
        Task<Response<dynamic>> AssignQuestionToQuiz(List<QuizQuestionRequest> quizQuestionRequests); //https://community.canvaslms.com/t5/Instructor-Guide/How-do-I-create-a-quiz-with-individual-questions/ta-p/1248
        Task<Response<dynamic>> DeleteQuestionFromQuiz(int quizQuestionId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Application.Response;

namespace LMS.Application.Contracts.Repositories
{
    public interface IQuestionBankServices
    {
        Task<Response<dynamic>> GetAllQuestions();
        Task<Response<QuestionBankResponse>> CreateQuestion(QuestionBankRequest question);
        Task<Response<QuestionBankResponse>> GetQuestionsById(int id);
        Task<Response<QuestionBankResponse>> UpdateQuestion(QuestionBankDto question);
        Task<Response<dynamic>> DeleteQuestionById(int id);

    }
}

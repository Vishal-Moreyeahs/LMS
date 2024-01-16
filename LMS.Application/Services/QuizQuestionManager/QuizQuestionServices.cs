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

namespace LMS.Application.Services.QuizQuestionManager
{
    public class QuizQuestionServices : IQuizQuestionServices
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public QuizQuestionServices(IAuthenticatedUserService authenticatedUserService, IMapper mapper, IUnitOfWork unitOfWork)
        { 
            _authenticatedUserService = authenticatedUserService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<dynamic>> AssignQuestionToQuiz(List<QuizQuestionRequest> quizQuestionRequests)
        {
            if (quizQuestionRequests.Count == 0 || quizQuestionRequests == null)
            { 
                throw new ApplicationException("Invalid Request Format");
            }

            //await _unitOfWork.GetRepository<QuizQuestion>()

            return null;
        }

        public async Task<Response<dynamic>> DeleteQuestionFromQuiz(int quizQuestionId)
        {
            var quizQuestion = await _unitOfWork.GetRepository<QuizQuestion>().Get(quizQuestionId);
            if (quizQuestion == null || !quizQuestion.IsActive)
            {
                throw new ApplicationException("Quiz question not found");
            }

            quizQuestion.IsActive = false;
            await _unitOfWork.GetRepository<QuizQuestion>().Update(quizQuestion);
            var isQuizQuestionDeleted = await _unitOfWork.SaveChangesAsync();

            if (isQuizQuestionDeleted <= 0)
            {
                throw new ApplicationException("Quiz Question not deleted");
            }

            var response = new Response<dynamic>
            {
                Status = true,
                Message = "Quiz question deleted successfully", 
            };
            return response;
        }

        public async Task<Response<dynamic>> GetQuizQuestions(int quizId)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            var quiz = await _unitOfWork.GetRepository<Quiz>().Get(quizId);

            if (quiz == null || !quiz.IsActive || quiz.Company_Id != loggedInUser.CompanyId)
            {
                throw new ApplicationException("Quiz not found");
            }

            var quizQuestion = _unitOfWork.GetRepository<QuizQuestion>().GetAllRelatedEntity();

            quizQuestion = quizQuestion.Where(x => x.Quiz_Id == quizId && x.IsActive).OrderBy(x => x.SequenceNo).ToList();

            var questions = quizQuestion.Select(x => x.QuestionBank).ToList();

            var response = new Response<dynamic> { 
                Status = true,
                Message = "Question for Quiz is retreived successfully",
                Data = questions
            };
            return response;
        }
    }
}

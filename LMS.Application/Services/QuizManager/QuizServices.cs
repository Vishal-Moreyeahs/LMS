using AutoMapper;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Domain.Models;

namespace LMS.Application.Services.QuizManager
{
    public class QuizServices : IQuizServices
    {
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IUnitOfWork _unitOfWork;

        public QuizServices(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUserService = authenticatedUserService;
            _mapper = mapper;
        }

        public async Task<Response<dynamic>> CreateQuiz(QuizRequest quiz)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            var data = _mapper.Map<Quiz>(quiz);
            data.Company_Id = loggedInUser.CompanyId;

            await _unitOfWork.GetRepository<Quiz>().Add(data);
            var isDataSaved = await _unitOfWork.SaveChangesAsync();

            if (isDataSaved <= 0)
            {
                throw new ApplicationException($"Fail to add quiz");
            }
            var response =  new Response<dynamic> { 
                Status = true,
                Message = "Quiz added successfully."
            };
            return response;
        }

        public async Task<Response<dynamic>> DeleteQuizById(int id)
        {
            var loggedInUser = _authenticatedUserService.GetLoggedInUser();

            var quiz = await _unitOfWork.GetRepository<Quiz>().Get(id);
            if (quiz == null)
            {
                throw new ApplicationException($"Quiz with id - {id} not exist");
            }

            quiz.IsActive = false;
            await _unitOfWork.GetRepository<Quiz>().Update(quiz);
            var isQuizDeleted = await _unitOfWork.SaveChangesAsync();

            if (isQuizDeleted <= 0)
            {
                throw new ApplicationException("Quiz not deleted");
            }
            var response = new Response<dynamic> { 
                Status= true,
                Message = "Quiz Deleted Successfully."
            };
            return response;
        }

        public async Task<Response<dynamic>> GetAllQuizes()
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            var quizzes = _unitOfWork.GetRepository<Quiz>().GetAllRelatedEntity();

            if (quizzes == null)
            {
                throw new ApplicationException("Questions not found in question bank");
            }

            quizzes = quizzes.Where(x => x.IsActive && x.Company_Id == loggedInUser.CompanyId);

            var response = new Response<dynamic>
            {
                Status = true,
                Message = "All Quizes Retreived",
                Data = quizzes
            };

            return response;
        }

        public async Task<Response<dynamic>> GetQuizById(int id)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            var quiz = await _unitOfWork.GetRepository<Quiz>().Get(id);

            if (quiz == null || quiz.Company_Id != loggedInUser.CompanyId || !quiz.IsActive)
            {
                throw new ApplicationException($"Quiz with id - {id} not found");
            }

            var response = new Response<dynamic> { Status = true, Message = "Quiz Retrieved Successfully", Data = quiz };
            return response;
        }

        public async Task<Response<dynamic>> UpdateQuiz(QuizDto quiz)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            var quizData = await _unitOfWork.GetRepository<Quiz>().Get(quiz.Id);

            if (quizData == null || !quizData.IsActive || quizData.Company_Id != loggedInUser.CompanyId)
            {
                throw new ApplicationException("Quiz not found");
            }

            _mapper.Map(quiz, quizData);
            
            await _unitOfWork.GetRepository<Quiz>().Update(quizData);
            var isQuizUpdated = await _unitOfWork.SaveChangesAsync();

            if (isQuizUpdated <= 0)
            {
                throw new ApplicationException("Quiz not updated");
            }

            var response = new Response<dynamic>
            { 
                Status = true,
                Message = "Quiz updated successfully",
                Data = quizData
            };
            return response;
        }
    }
}

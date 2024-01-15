using AutoMapper;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

            return null;
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
    }
}

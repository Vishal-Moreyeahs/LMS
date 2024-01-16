using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Models;

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
        public Task<Response<dynamic>> AssignQuestionToQuiz()
        {
            throw new NotImplementedException();
        }

        public Task<Response<dynamic>> DeleteQuestionFromQuiz()
        {
            throw new NotImplementedException();
        }

        public Task<Response<dynamic>> GetQuizQuestions(int quizId)
        {
            var 
        }
    }
}

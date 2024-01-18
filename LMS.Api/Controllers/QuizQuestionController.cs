using LMS.Application.Contracts.Repositories;
using LMS.Application.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizQuestionController : ControllerBase
    {
        private readonly IQuizQuestionServices _quizQuestionServices;

        public QuizQuestionController(IQuizQuestionServices quizQuestionServices)
        { 
            _quizQuestionServices = quizQuestionServices;
        }

        [HttpGet]
        [Route("getQuizQuestionsByQuizId")]
        public async Task<IActionResult> GetQuestionByQuizId(int quizId)
        {
            return Ok(await _quizQuestionServices.GetQuizQuestions(quizId));
        }

        [HttpDelete]
        [Route("DeleteQuestionFromQuizById")]
        public async Task<IActionResult> DeleteQuestionByQuizId(int quizQuestionId)
        {
            return Ok(await _quizQuestionServices.DeleteQuestionFromQuiz(quizQuestionId));
        }

        [HttpPost]
        [Route("assignQuestionToQuiz")]
        public async Task<IActionResult> AssignQuestionToQuiz(List<QuizQuestionRequest> quizQuestionRequests)
        {
            return Ok(await _quizQuestionServices.AssignQuestionToQuiz(quizQuestionRequests));
        }
    }
}

using LMS.Application.Contracts.Repositories;
using LMS.Application.Request;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizServices _quizServices;

        public QuizController(IQuizServices quizServices)
        {
            _quizServices = quizServices;
        }

        [HttpGet]
        [Route("getAllQuizes")]
        public async Task<IActionResult> GetAllQuizes()
        {
            return Ok(await _quizServices.GetAllQuizes());
        }

        [HttpGet]
        [Route("getQuizById")]
        public async Task<IActionResult> GetQuizById(int id)
        {
            return Ok(await _quizServices.GetQuizById(id));
        }

        [HttpPost]
        [Route("CreateQuiz")]
        public async Task<IActionResult> CreateQuiz(QuizRequest quizRequest)
        { 
            return Ok(await _quizServices.CreateQuiz(quizRequest));
        }

        [HttpDelete]
        [Route("DeleteQuizById")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            return Ok(await _quizServices.DeleteQuizById(id));
        }

        [HttpPost]
        [Route("UpdateQuiz")]
        public async Task<IActionResult> UpdateQuiz(QuizDto quiz)
        {
            return Ok(await _quizServices.UpdateQuiz(quiz));
        }
    }
}

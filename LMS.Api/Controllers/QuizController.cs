using LMS.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> GetAllQuizzes()
        {
            return Ok(await _quizServices.GetAllQuizes());
        }

        [HttpGet]
        [Route("getAllQuizes")]
        public async Task<IActionResult> GetQuizById(int id)
        {
            return Ok(await _quizServices.GetQuizById(id));
        }
    }
}

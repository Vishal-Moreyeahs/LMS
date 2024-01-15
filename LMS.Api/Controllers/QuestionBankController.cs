using LMS.Application.Contracts.Repositories;
using LMS.Application.Models.Blob;
using LMS.Application.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionBankController : ControllerBase
    {
        private readonly IQuestionBankServices _questionBankServices;

        public QuestionBankController(IQuestionBankServices questionBankServices)
        {
            _questionBankServices = questionBankServices;
        }

        [HttpGet]
        [Route("getAllQuestions")]
        public async Task<IActionResult> GetAllQuestions()
        {
            return Ok(await _questionBankServices.GetAllQuestions());
        }

        [HttpPost]
        [Route("addQuestion")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> AddQuestionToQuestionBank([FromForm] QuestionBankRequest question)
        {
            return Ok(await _questionBankServices.CreateQuestion(question));
        }

        [HttpGet]
        [Route("getQuestionById")]
        public async Task<IActionResult> GetQuestionsById(int id)
        {
            return Ok(await _questionBankServices.GetQuestionsById(id));
        }

        [HttpDelete]
        [Route("deleteQuestion")]
        public async Task<IActionResult> DeleteQuestionFromId(int id)
        {
            return Ok(await _questionBankServices.GetAllQuestions());
        }

        [HttpPost]
        [Route("updateQuestion")]
        public async Task<IActionResult> UpdateQuestion([FromForm] QuestionBankDto question)
        {
            return Ok();
        }
    }
}

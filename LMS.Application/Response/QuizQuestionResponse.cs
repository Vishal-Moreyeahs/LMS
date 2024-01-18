using LMS.Application.Request;

namespace LMS.Application.Response
{
    public class QuizQuestionResponse
    {
        public string Name { get; set; }
        public int SubDomain_Id { get; set; }
        public List<OptionRequest> Options { get; set; }
    }
}
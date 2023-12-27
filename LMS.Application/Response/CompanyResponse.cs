using System.ComponentModel.DataAnnotations;
using LMS.Application.Request;

namespace LMS.Application.Response
{
    public class CompanyResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
    }

    public class CompanyData : CompanyRequest
    {
        [Required]
        public int Id { get; set; }
    }
}

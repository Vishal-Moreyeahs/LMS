using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LMS.Application.Request
{
    public class OptionRequest
    {
        public string? OptionValue { get; set; }
        public bool IsImageAttached { get; set; }
        public IFormFile? ImageFile { get; set; }
        public bool IsCorrect { get; set; }
        public int? QuestionBank_Id { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

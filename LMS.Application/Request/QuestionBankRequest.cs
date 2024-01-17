using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LMS.Application.Response;
using LMS.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace LMS.Application.Request
{
    public class QuestionBankRequest
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsImageAttached { get; set; } = false;

        [JsonPropertyName("imagePath")]
        public IFormFile? ImageFile { get; set; }
        public bool isActive { get; set; } = true;
        public List<OptionRequest> Options { get; set; }

        [Required]
        public int SubDomain_Id { get; set; }
    }
    public class QuestionBankDto : QuestionBankRequest
    {
        public int Id { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Request;
using Microsoft.AspNetCore.Http;

namespace LMS.Application.Response
{
    public class QuestionBankResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsImageAttached { get; set; }
        public string? ImagePath { get; set; }
        public int SubDomain_Id { get; set; }
        public List<OptionRequest> Options { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LMS.Application.Request
{
    public class FileBankDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }
        public string Format { get; set; }
        public string? Path { get; set; }
    }
}

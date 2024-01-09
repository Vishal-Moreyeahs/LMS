using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LMS.Application.Request
{
    public class UploadFileBankRequest
    {
        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}

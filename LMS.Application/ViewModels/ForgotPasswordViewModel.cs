using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Enter Email")]
        public string EmailAddress { get; set; }

    }
}

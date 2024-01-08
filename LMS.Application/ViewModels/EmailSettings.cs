using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.ViewModels
{
    public class EmailSettings
    {
        public string EmailFrom { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }
        public string Password { get; set; }
        public string VerifyRegistrationUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string VerifyResetPasswordUrl { get; set; }

    }
}

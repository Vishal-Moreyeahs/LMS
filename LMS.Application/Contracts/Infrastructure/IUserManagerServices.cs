using LMS.Application.Models;
using LMS.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Contracts.Infrastructure
{
    public interface IUserManagerServices
    {
        void ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel);

        Response<bool> ResetPassword(ResetPasswordViewModel resetPasswordViewModel);
        void ResetPassword();
    }
}

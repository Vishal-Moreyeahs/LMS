using LMS.Application.Contracts.Infrastructure;
using LMS.Application.Contracts.Persistence;
using LMS.Application.ViewModels;
using LMS.Domain.Models;
using LMS.Infrastructure.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LMS.Infrastructure.Services
{
    public class UserManagerServices : IUserManagerServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailSettings _emailSettings;
        public UserManagerServices(IUnitOfWork unitOfWork, IOptions<EmailSettings> emailSettings)
        {
            _unitOfWork = unitOfWork;
            _emailSettings = emailSettings.Value;
        }

        public string ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            var users = _unitOfWork.GetRepository<Employee>().GetAll().Result;
            var user = users.Where(x => x.Email == forgotPasswordViewModel.EmailAddress).FirstOrDefault();
            if (user == null)
            {
                throw new ApplicationException($"User does not Exists");
            }
            var res = SendVerificationEmail(user,"nsdkjfasdngkndgjansgfbasgjnsangjaib"); 
            return res;
        }

        public void ResetPassword()
        {
            throw new NotImplementedException();
        }

        //private void Send(Employee user)
        //{
        //    var emailVerficationToken = GenerateHashSha256.ComputeSha256Hash((GenerateRandomNumbers.RandomNumbers(6)));
        //    _verificationRepository.SendResetVerificationToken(user.UserId, emailVerficationToken);

        //    MailMessage message = new MailMessage();
        //    SmtpClient smtpClient = new SmtpClient();
        //    try
        //    {
        //        MailAddress fromAddress = new MailAddress(_appSettings.EmailFrom);
        //        message.From = fromAddress;
        //        message.To.Add(user.Email);
        //        message.Subject = "Welcome to Web Secure";
        //        message.IsBodyHtml = true;
        //        message.Body = SendVerificationEmail(user, emailVerficationToken);
        //        smtpClient.Host = _appSettings.Host;
        //        smtpClient.Port = Convert.ToInt32(_appSettings.Port);
        //        smtpClient.EnableSsl = true;
        //        smtpClient.UseDefaultCredentials = false;
        //        smtpClient.Credentials = new System.Net.NetworkCredential(_appSettings.EmailFrom, _appSettings.Password);
        //        smtpClient.Send(message);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public string SendVerificationEmail(Employee user, string token)
        {
            AesAlgorithm aesAlgorithm = new AesAlgorithm();
            var key = string.Join(":", new string[] { DateTime.Now.Ticks.ToString(), user.Id.ToString() });
            var encrypt = aesAlgorithm.EncryptToBase64String(key);

            var linktoverify = _emailSettings.VerifyResetPasswordUrl + "?key=" + HttpUtility.UrlEncode(encrypt) + "&hashtoken=" + HttpUtility.UrlEncode(token);
            var stringtemplate = new StringBuilder();
            stringtemplate.Append("Welcome");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("Dear " + string.Concat(user.FirstName," ",user.LastName));
            stringtemplate.Append("<br/>");
            stringtemplate.Append("Please click the following link to reset your password.");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("Reset password link : <a target='_blank' href=" + linktoverify + ">Link</a>");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("If the link does not work, copy and paste the URL into a new browser window. The URL will expire in 24 hours for security reasons.");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("Best regards,");
            stringtemplate.Append("Saineshwar Begari");
            stringtemplate.Append("<br/>");
            return stringtemplate.ToString();
        }
    }
}

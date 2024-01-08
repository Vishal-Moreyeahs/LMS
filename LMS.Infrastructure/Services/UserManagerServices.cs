using LMS.Application.Contracts.Infrastructure;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Models;
using LMS.Application.ViewModels;
using LMS.Domain.Models;
using LMS.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly ICryptographyService _cryptographyService;
        private readonly IAuthService _authService;
        private readonly IMailServices _mailServices;
        public UserManagerServices(IUnitOfWork unitOfWork, IOptions<EmailSettings> emailSettings, IAuthService authService
            , IMailServices mailServices, ICryptographyService cryptographyService)
        {
            _unitOfWork = unitOfWork;
            _emailSettings = emailSettings.Value;
            _authService = authService;
            _mailServices = mailServices;
            _cryptographyService = cryptographyService;
        }

        public void ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            try
            {
                var users = _unitOfWork.GetRepository<Employee>().GetAll().Result;
                var user = users.Where(x => x.Email == forgotPasswordViewModel.EmailAddress).FirstOrDefault();
                if (user == null)
                {
                    throw new ApplicationException($"User does not Exists");
                }
                Send(user);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ResetPassword()
        {
            throw new NotImplementedException();
        }

        public Response<bool> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            var userdetails = _unitOfWork.GetRepository<Employee>().Get(resetPasswordViewModel.UserId).Result;
            if (userdetails == null)
            {
                throw new ApplicationException("User does not Exists");
            }
            var password = _cryptographyService.EncryptPassword(userdetails.Email + resetPasswordViewModel.Password);
            userdetails.Password = password;
            _unitOfWork.GetRepository<Employee>().Update(userdetails);
            var isReset = _unitOfWork.Save().Result;
            if (isReset <= 0)
            {
                throw new ApplicationException("Password Not Updated. Please try later");
            }
            var tokens = _unitOfWork.GetRepository<ResetPasswordVerification>().GetAll().Result;
            var resetPasswordVerification = tokens.Where(x => x.GeneratedToken == resetPasswordViewModel.Token && x.VerificationStatus == false).FirstOrDefault();
            if (resetPasswordVerification == null)
            {
                throw new ApplicationException("Invalid Request !!!");
            }
            resetPasswordVerification.VerificationStatus = true;
            _unitOfWork.GetRepository<ResetPasswordVerification>().Add(resetPasswordVerification);
            _unitOfWork.Save();
            var response = new Response<bool>
            {
                Status = true,
                Message = "Reset Password Successfully",
                Data = true
            };
            return response;
        }

        public void Send(Employee user)
        {
            var emailVerficationToken = _authService.GenerateToken(user).Result;
            var token = new JwtSecurityTokenHandler().WriteToken(emailVerficationToken);

            ResetPasswordVerification model = new ResetPasswordVerification
            {
                GeneratedToken = token,
                GeneratedDate = DateTime.Now,
                UserId = user.Id,
                VerificationStatus = false
            };

            _unitOfWork.GetRepository<ResetPasswordVerification>().Add(model);
            _unitOfWork.Save();
            var body = SendVerificationEmail(user, token);

            _mailServices.SendEmailAsync(user.Email, "Reset Password Link", body);

            //MailMessage message = new MailMessage();
            //  SmtpClient smtpClient = new SmtpClient();
            //  try
            //  {
            //      MailAddress fromAddress = new MailAddress(_emailSettings.EmailFrom);
            //      message.From = fromAddress;
            //      message.To.Add("Vishal.Pawar@moreyeahs.com");
            //      message.Subject = "Welcome to Web Secure";
            //      message.IsBodyHtml = true;
            //      message.Body = SendVerificationEmail(user, token);
            //      SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);
            //      smtp.Timeout = 1000000;
            //      //smtpClient.Host = _emailSettings.Host;
            //      //smtpClient.Port = Convert.ToInt32(_emailSettings.Port);
            //      smtpClient.EnableSsl = true;
            //      smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //      smtpClient.UseDefaultCredentials = false;
            //      smtpClient.Credentials = new System.Net.NetworkCredential("Vishal.Pawar@moreyeahs.com", "Vageta@123");
            //      smtpClient.Send(message);
            //  }
            //  catch (Exception)
            //  {
            //      throw;
            //  }
        }


        public string SendVerificationEmail(Employee user, string token)
        {
            //AesAlgorithm aesAlgorithm = new AesAlgorithm();
            //var key = string.Join(":", new string[] { DateTime.Now.Ticks.ToString(), user.Id.ToString() });
            //var encrypt = aesAlgorithm.EncryptToBase64String(key);

            var linktoverify = _emailSettings.VerifyResetPasswordUrl + "?userId=" + user.Id + "&token=" + token;
            var stringtemplate = new StringBuilder();
            stringtemplate.Append("Welcome");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("Dear " + string.Concat(user.FirstName, " ", user.LastName));
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

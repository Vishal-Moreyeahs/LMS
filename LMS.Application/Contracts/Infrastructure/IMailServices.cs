using Microsoft.AspNetCore.Http;

namespace LMS.Application.Contracts.Infrastructure
{
    public interface IMailServices
    {
        Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachments = null);
    }

}

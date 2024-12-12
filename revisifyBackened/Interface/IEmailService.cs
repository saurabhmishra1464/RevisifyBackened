using revisifyBackened.Models.Dto;

namespace revisifyBackened.Interface
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailRequest request);
    }
}

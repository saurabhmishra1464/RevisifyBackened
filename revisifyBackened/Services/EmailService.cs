using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Utils;
using revisifyBackened.Interface;
using revisifyBackened.Models.Dto;

namespace revisifyBackened.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;
        private readonly IHostEnvironment _env;
        public EmailService(ILogger<EmailService> logger, IOptions<EmailSettings> settings, IHostEnvironment env)
        {
            _logger = logger;
            _settings = settings.Value;
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        //private void AddAttachments(EmailRequest request, BodyBuilder bodyBuilder)
        //{
        //    if (request.Attachments != null && request.Attachments.Any())
        //    {
        //        foreach (var attachment in request.Attachments)
        //        {
        //            try
        //            {
        //                byte[] fileBytes = Convert.FromBase64String(attachment.Base64Content);
        //                bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
        //            }
        //            catch (FormatException ex)
        //            {
        //                // Log or handle the base64 decoding error
        //                _logger.LogError(ex, "Format Exception duting adding the attachment");
        //            }
        //            catch (Exception ex)
        //            {
        //                // Log or handle other errors
        //                _logger.LogError(ex, "Error adding attachment", attachment.FileName, ex.Message);
        //            }
        //        }
        //    }
        //}


        public async Task<bool> SendEmailAsync(EmailRequest request)
        {
            MimeMessage message = CreateEmailMessage(request);

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, false);
                await client.AuthenticateAsync(_settings.Username, _settings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            _logger.LogInformation("Email sent successfully to {Recipient}", string.Join(", ", message.To));
            return true;
        }



        private MimeMessage CreateEmailMessage(EmailRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Email request cannot be null.");

            if (request.ToList == null || !request.ToList.Any())
                throw new ArgumentException("Recipient list cannot be empty.", nameof(request.ToList));

            //string imagePath = Path.Combine(_env.ContentRootPath, "Assets", "Logo.png");
            //if (!File.Exists(imagePath))
            //    throw new FileNotFoundException($"The file '{imagePath}' does not exist.");
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Revisify", _settings.DefaultFrom));
            //var mailboxaddress = message.ToList.Select(address => new MailboxAddress(address)).ToList();
            var mailboxAddresses = request.ToList.Select(address => new MailboxAddress(string.Empty, address)).ToList();
            emailMessage.To.AddRange(mailboxAddresses);
            //emailMessage.Cc.AddRange(request.CcList.Select(cc => new MailboxAddress(string.Empty, cc)).ToList());
            emailMessage.Subject = request.Subject;
            var bodyBuilder = new BodyBuilder { HtmlBody = request.HtmlBody };
            //var image = bodyBuilder.LinkedResources.Add(imagePath);
            //image.ContentId = "logo_cid";
            //image.ContentDisposition = new ContentDisposition(ContentDisposition.Inline);
            //image.ContentType.MediaType = "image";
            //image.ContentType.MediaSubtype = "png";

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }
    }
}

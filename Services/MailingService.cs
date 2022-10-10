using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SendEmailsDotNetCore6.Settings;

namespace SendEmailsDotNetCore6.Services
{
    public class MailingService : IMailingService
    {
        private readonly MailSettings _mailSettings;
        public MailingService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachments = null)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = subject,
            };

            foreach (string s in mailTo.Split(","))
                email.To.Add(MailboxAddress.Parse(s));



            var builder = new BodyBuilder();

            if(attachments != null)
            {
                byte[] fileByte;
                foreach (var file in attachments)
                {
                    if(file.Length > 0)
                    {
                       using var ms = new MemoryStream();
                        file.CopyTo(ms);
                        fileByte = ms.ToArray();

                        builder.Attachments.Add(file.FileName, fileByte, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.SslOnConnect);
            smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}

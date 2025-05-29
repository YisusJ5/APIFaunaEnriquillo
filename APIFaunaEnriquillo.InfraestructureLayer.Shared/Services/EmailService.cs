using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Email;
using Microsoft.Extensions.Options;
using APIFaunaEnriquillo.Core.DomainLayer.Setting;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service;
using MimeKit;
using MailKit.Security;

namespace APIFaunaEnriquillo.InfraestructureLayer.Shared.Services
{
    public class EmailService(IOptions<EmailSetting> emailSettings) : IEmailService
    {
        private EmailSetting _emailSetting { get; } = emailSettings.Value;
        public async Task SendAsync(EmailRequestDto Request)
        {
            try
            {
                MimeMessage email = new ();
                email.Sender = MailboxAddress.Parse(_emailSetting.EmailFrom);
                email.To.Add(MailboxAddress.Parse(Request.To));
                email.Subject = Request.Subject;
                BodyBuilder builder = new();
                builder.HtmlBody = Request.Body;
                email.Body = builder.ToMessageBody();
                 using MailKit.Net.Smtp.SmtpClient smtp = new();
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smtp.Connect(_emailSetting.SmtpHost, _emailSetting.SmtpPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailSetting.SmtpUser, _emailSetting.SmtpPass);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

            }
            catch(Exception)
            {

            }
        }
    }
}

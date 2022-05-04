using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Tienda365.BL.Config;
using Tienda365.BL.Interface;
using Tienda365.BL.Models;

namespace Tienda365.BL.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _emailConfig;
        private const string templatePath = @"EmailTemplate/{0}.html";

        public EmailService(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task SendEmailForForgotPassword(UserEmailOptions options)
        {
            options.Subject = UpdatePlaceHolders("Hello {{UserName}}, reset your password.", options.PlaceHolders);

            options.Body = UpdatePlaceHolders(GetEmailBody("ForgotPassword"), options.PlaceHolders);

            await SendEmail(options);
        }

        private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }

            return text;
        }

        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }

        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {
            Console.WriteLine(_emailConfig);
            MailMessage mail = new MailMessage
            {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(_emailConfig.SenderAddress, _emailConfig.SenderDisplayName),
                IsBodyHtml = _emailConfig.IsBodyHTML
            };

            foreach (var toEmail in userEmailOptions.ToEmails)
            {
                mail.To.Add(toEmail);
            }

            NetworkCredential networkCredential = new NetworkCredential(_emailConfig.UserName, _emailConfig.Password);

            SmtpClient smtpClient = new SmtpClient
            {
                Host = _emailConfig.Host,
                Port = _emailConfig.Port,
                EnableSsl = _emailConfig.EnableSSL,
                UseDefaultCredentials = _emailConfig.UseDefaultCredentials,
                Credentials = networkCredential,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            mail.BodyEncoding = Encoding.Default;

            await smtpClient.SendMailAsync(mail);
        }
    }
}

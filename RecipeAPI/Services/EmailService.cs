﻿using Microsoft.Extensions.Options;
using RecipeAPI.Constants;
using RecipeAPI.Models;
using RecipeAPI.Services.IServices;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RecipeAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly SMTPConfigModel _smtpConfig;
        private readonly SendGridConfigModel _sendGridConfig;

        public EmailService(IOptions<SMTPConfigModel> smtpConfig, IOptions<SendGridConfigModel> sendGridConfig)
        {
            _smtpConfig = smtpConfig.Value;
            _sendGridConfig = sendGridConfig.Value;
        }

        public async Task SendEmailForEmailConfirmation(UserEmailOptionsModel userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Hello {{UserName}}, This is confirmation link for your registration.", userEmailOptions.PlaceHolders);

            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("EmailConfirmation"), userEmailOptions.PlaceHolders);

            await SendEmail(userEmailOptions);
        }

        public async Task SendEmailForForgotPassword(UserEmailOptionsModel userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Hello {{UserName}}, reset your password.", userEmailOptions.PlaceHolders);

            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("ForgotPassword"), userEmailOptions.PlaceHolders);

            await SendEmail(userEmailOptions);
        }

        private async Task SendEmail(UserEmailOptionsModel userEmailOptions)
        {
            //SendGrid
            foreach (var toEmail in userEmailOptions.ToEmails)
            {
                var apiKey = _sendGridConfig.APIKey;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(_sendGridConfig.SenderEmail, _sendGridConfig.SenderName);
                var subject = userEmailOptions.Subject;
                var to = new EmailAddress(toEmail, toEmail);
                var plainTextContent = string.Empty;
                var htmlContent = userEmailOptions.Body;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }

            if (true)
            {
                //Basic SMTP
                MailMessage mail = new MailMessage
                {
                    Subject = userEmailOptions.Subject,
                    Body = userEmailOptions.Body,
                    From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                    IsBodyHtml = _smtpConfig.IsBodyHTML
                };

                foreach (var toEmail in userEmailOptions.ToEmails)
                {
                    mail.To.Add(toEmail);
                }

                NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password);
                SmtpClient smtpClient = new SmtpClient
                {
                    Host = _smtpConfig.Host,
                    Port = _smtpConfig.Port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    UseDefaultCredentials = _smtpConfig.UseDefaultCredentials,
                    Credentials = networkCredential
                };
                mail.BodyEncoding = Encoding.Default;
                await smtpClient.SendMailAsync(mail);
            }
        }

        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(AppConstants.EmailTemplatesPath, templateName));
            return body;
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
    }
}

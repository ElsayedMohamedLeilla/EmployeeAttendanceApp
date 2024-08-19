using Dawem.Contract.BusinessLogic.Dawem.Provider;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using NuGet.Packaging;
using System.Net;
using System.Net.Mail;

namespace Dawem.BusinessLogic.Dawem.Provider
{
    public class MailBL : IMailBL
    {
        private readonly RequestInfo requestHeaderContext;
        public MailBL(RequestInfo _requestHeaderContext)
        {

            requestHeaderContext = _requestHeaderContext;
        }

        public async Task<bool> SendEmail(VerifyEmailModel emailModel)
        {
            if (emailModel.Email != null)
            {
                var isValidEmail = EmailHelper.IsValidEmail(emailModel.Email);
                if (!isValidEmail)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryYouMustEnterValidEmail);
                }
            }
            if (emailModel.Emails != null)
            {
                emailModel.Emails.ForEach(email =>
                {
                    var isValidEmail = EmailHelper.IsValidEmail(email);
                    if (!isValidEmail)
                    {
                        throw new BusinessValidationException(LeillaKeys.SorryYouMustEnterValidEmail);
                    }
                });
            }

            // Create a new mail message
            MailMessage message = new()
            {
                // Set the sender and recipient addresses
                From = new MailAddress(LeillaKeys.DawemAppDevelopersGmailCom)
            };

            if (emailModel.Email != null)
            {
                message.To.Add(emailModel.Email);
            }
            if (emailModel.Emails != null)
            {
                var emails = emailModel.Emails.Select(email => new MailAddress(email));
               
                message.To.Add(LeillaKeys.DawemAppDevelopersGmailCom);
                //message.To.AddRange(emails);
                message.Bcc.AddRange(emails);
            }

            message.IsBodyHtml = true;
            // Set the subject and body of the message
            message.Subject = emailModel.Subject;
            message.Body = emailModel.Body;

            var client = new SmtpClient(LeillaKeys.SmtpGmailCom, 587)
            {
                Credentials = new NetworkCredential(LeillaKeys.DawemAppDevelopersGmailCom, LeillaKeys.DawemAppDevelopersGmailComPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            try
            {
                await client.SendMailAsync(message);
            }
            catch (Exception e)
            {
                throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhenSendEmail);
            }

            return true;
        }


    }
}

using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Shared;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using System.Net;
using System.Net.Mail;

namespace Dawem.BusinessLogic.Provider
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
            var isValidEmail = EmailHelper.IsValidEmail(emailModel.Email);
            if (!isValidEmail)
            {
                throw new BusinessValidationException(DawemKeys.SorryYouMustEnterValidEmail);
            }

            // Create a new mail message
            MailMessage message = new()
            {
                // Set the sender and recipient addresses
                From = new MailAddress(DawemKeys.DawemAppDevelopersGmailCom)
            };
            message.To.Add(emailModel.Email);
            message.IsBodyHtml = true;
            // Set the subject and body of the message
            message.Subject = emailModel.Subject;
            message.Body = emailModel.Body;

            var client = new SmtpClient(DawemKeys.SmtpGmailCom, 587)
            {
                Credentials = new NetworkCredential(DawemKeys.DawemAppDevelopersGmailCom, DawemKeys.DawemAppDevelopersGmailComPassword),
                EnableSsl = true
            };

            try
            {
                await client.SendMailAsync(message);
            }
            catch
            {
                throw new BusinessValidationException(DawemKeys.SorryErrorHappenWhenSendEmail);
            }

            return true;
        }


    }
}

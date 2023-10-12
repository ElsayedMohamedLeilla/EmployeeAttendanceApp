using SmartBusinessERP.BusinessLogic.Provider.Contract;
using SmartBusinessERP.BusinessLogic.Validators.FluentValidators;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Helpers;
using SmartBusinessERP.Models.Context;
using SmartBusinessERP.Models.Dtos.Shared;
using SmartBusinessERP.Models.Response;
using System.Net;
using System.Net.Mail;

namespace SmartBusinessERP.BusinessLogic.Provider
{
    public class MailBL : IMailBL
    {
        private readonly RequestHeaderContext userContext;
        public MailBL(RequestHeaderContext _userContext)
        {

            userContext = _userContext;
        }

        public async Task<BaseResponseT<bool>> SendEmail(VerifyEmailModel emailModel)
        {
            var result = new BaseResponseT<bool>();

            var validator = new EmailValidator(userContext);
            var response = validator.Validate(emailModel);
            if (!response.IsValid)
            {


                result.Status = ResponseStatus.ValidationError;
                result.MessageCode = response.Errors.Select(x => x.ErrorCode).FirstOrDefault();
                result.Message = response.Errors[0].ErrorMessage;
                return result;
            }



            // Create a new mail message
            MailMessage message = new()
            {
                // Set the sender and recipient addresses
                From = new MailAddress("smart.business.erp.developers@gmail.com")
            };
            message.To.Add(emailModel.Email);
            message.IsBodyHtml = true;
            // Set the subject and body of the message
            message.Subject = emailModel.Subject;
            message.Body = emailModel.Body;





            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("smart.business.erp.developers@gmail.com", "jmjjjrxvdkennqdv"),
                EnableSsl = true
            };

            try
            {
                // Send the message
                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                TranslationHelper.SetValidationMessages(result, "Email_Error", "Email not sent", lang: "en");
                result.Status = ResponseStatus.Error;
                return result;
            }
            result.Status = ResponseStatus.Success;
            result.Result = true;
            return result;
        }


    }
}

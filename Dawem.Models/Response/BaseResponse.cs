using Dawem.Enums.General;
using Dawem.Models.Shared;

namespace Dawem.Models.Response
{
    public class BaseResponse
    {

        public BaseResponse()
        {

        }
        public BaseResponse(string lang = DawemKeys.Ar)
        {
            Lang = lang;
            Message = lang == DawemKeys.Ar ? "تم بنجاح" : "Done Successfully";
        }
        public int TotalCount { get; set; }
        private ResponseStatus _status;
        private readonly string? Lang;
        public virtual ResponseStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                if (_status != ResponseStatus.Success && string.IsNullOrEmpty(Message))
                {
                    Message = string.IsNullOrWhiteSpace(Message) || string.IsNullOrEmpty(Message) ? Lang == DawemKeys.En ? "Error has occurred" : " حدث  خطأ" : Message;
                }
                else if (_status != ResponseStatus.Success)
                {
                    Message = string.IsNullOrWhiteSpace(Message) || string.IsNullOrEmpty(Message) ? Lang == DawemKeys.En ? "Sorry try again later" : "لا يمكن اجراء هذا الحدث" : Message;
                }
            }
        }
        public virtual string? Title { get; set; }
        public virtual string Message { get; set; } = "تم بنجاح";
        public virtual string? MessageCode { get; set; }
        public virtual List<MetaPair>? DetailedMessages { get; set; }
        public virtual Exception Exception { get; set; }



        //public ValidationResult ValidationResult { get; set; }

    }
}

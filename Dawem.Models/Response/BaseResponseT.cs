using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Translations;

namespace Dawem.Models.Response
{
    public class BaseResponseT<T>
    {

        public BaseResponseT()
        {

        }
        public BaseResponseT(string lang = LeillaKeys.Ar)
        {
            Lang = lang;
            Message = lang == LeillaKeys.Ar ? "تم بنجاح" : "Done Successfully";
        }
        public int TotalCount { get; set; }

        private ResponseStatus _status;
        private readonly string Lang;
        public virtual ResponseStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                if (_status != ResponseStatus.Success && string.IsNullOrEmpty(Message))
                {
                    Message = string.IsNullOrWhiteSpace(Message) || string.IsNullOrEmpty(Message) ? Lang == LeillaKeys.En ? "Error has occurred" : " حدث  خطأ" : Message;
                }
                else if (_status != ResponseStatus.Success)
                {
                    Message = string.IsNullOrWhiteSpace(Message) || string.IsNullOrEmpty(Message) ? Lang == LeillaKeys.En ? "Sorry try again later" : "لا يمكن اجراء هذا الحدث" : Message;
                }
            }
        }
        public virtual List<MetaPair> DetailedMessages { get; set; }
        public T Result { get; set; }
        public virtual string Title { get; set; }
        public virtual string Message { get; set; } = "تم بنجاح";
        public virtual string MessageCode { get; set; }
        public virtual List<string> Messages { get; set; }
        public virtual Exception Exception { get; set; }

    }
}

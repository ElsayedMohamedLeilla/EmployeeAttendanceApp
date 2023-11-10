using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Shared;
using Dawem.Translations;
using FluentValidation.Results;

namespace Dawem.Models.Response
{
    public class ExecutionResponse<T>
    {
        public ExecutionResponse()
        {
        }
        public ExecutionResponse(string lang = LeillaKeys.Ar)
        {
            Lang = lang;
            Message = lang == LeillaKeys.Ar ? LeillaKeys.DoneSuccessfullyAr : LeillaKeys.DoneSuccessfullyEn;
        }
        ResponseStatus _state;
        string Lang;
        public virtual ResponseStatus State
        {
            get { return _state; }
            set
            {
                _state = value;
                if (_state != ResponseStatus.Success && string.IsNullOrEmpty(Message))
                {
                    Message = string.IsNullOrWhiteSpace(Message) || string.IsNullOrEmpty(Message) ? Lang == LeillaKeys.En ? LeillaKeys.ErrorHasOccurredEn : LeillaKeys.ErrorHasOccurredAr : Message;
                }
                else if (_state != ResponseStatus.Success)
                {
                    Message = string.IsNullOrWhiteSpace(Message) || string.IsNullOrEmpty(Message) ? Lang == LeillaKeys.En ? LeillaKeys.SorryTryAgainLaterEn : LeillaKeys.SorryTryAgainLaterAr : Message;
                }
            }
        }
        #region props.

        public T Result { get; set; }
        public virtual List<MetaPair> DetailedMessages { get; set; }
        public virtual string MessageCode { get; set; }
        public virtual string Message { get; set; } = "تم بنجاح";
        public virtual List<string> Messages { get; set; }
        Exception _exception;
        public int? TotalCount { get; set; }
        public virtual Exception Exception
        {
            get
            {

                return _exception;
            }
            set
            {
                _exception = value;
                //To Do Very Important
                //if (Message == "تم بنجاح" && _exception != null)
                //{
                //    Message = Lang == DawemKeys.Ar ? "  حدث  خطأ فى الخادم" : "Exception has occurred";

                //    var context = new GlamourContext();
                //    var dbSet = context.Set<ExceptionLog>();
                //    ExceptionLog entity = new ExceptionLog()
                //    {
                //        Message = _exception.Message,
                //        StackTrace = _exception.StackTrace,
                //        AddedDate = DateTime.Now
                //    };
                //    dbSet.Add(entity);
                //    context.SaveChanges();
            }

        }

        public ValidationResult ValidationResult { get; set; }

        public static void CopyExecutionResponse<T1, T2>(ExecutionResponse<T1> source, ExecutionResponse<T2> destination)
        {
            destination.State = source.State;
            destination.Message = source.Message;
            destination.MessageCode = source.MessageCode;
            destination.Exception = source.Exception;
            destination.DetailedMessages = source.DetailedMessages;
        }
    }

    #endregion
}


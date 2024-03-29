using Dawem.Contract.BusinessLogic.Dawem.General;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Response.Dawem.Schedules.WeekDays;

namespace Dawem.BusinessLogic.Dawem.General
{
    public class GeneralBL : IGeneralBL
    {
        private readonly RequestInfo requestInfo;
        public GeneralBL(RequestInfo _requestInfo)
        {
            requestInfo = _requestInfo;
        }
        public List<GetWeekDaysDTO> GetWeekDays()
        {
            var weekDaysList = Enum.GetValues(typeof(WeekDay)).Cast<WeekDay>().ToList();
            var result = new List<GetWeekDaysDTO>();

            foreach (var weekDay in weekDaysList)
            {
                result.Add(new GetWeekDaysDTO
                {
                    WeekDay = weekDay,
                    Name = TranslationHelper.GetTranslation(weekDay.ToString(), requestInfo?.Lang)
                });
            }

            return result;

        }
    }
}


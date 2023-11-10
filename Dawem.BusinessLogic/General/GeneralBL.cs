using Dawem.Contract.BusinessLogic.General;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Response.Employees.Attendances.WeekDays;

namespace Dawem.BusinessLogic.General
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


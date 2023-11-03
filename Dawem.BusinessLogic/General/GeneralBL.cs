using Dawem.Contract.BusinessLogic.Employees.Department;
using Dawem.Enums.General;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Response.Employees.Attendances.WeeksAttendances;

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
            var weekDaysList = Enum.GetValues(typeof(WeekDays)).Cast<WeekDays>().ToList();
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


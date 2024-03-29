using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Dawem.Employees.HolidayTypes;

namespace Dawem.Models.AutoMapper.Dawem
{
    public class HolidayTypesMapProfile : Profile
    {
        public HolidayTypesMapProfile()
        {
            CreateMap<CreateHolidayTypeModel, HolidayType>();
            CreateMap<UpdateHolidayTypeModel, HolidayType>();
        }
    }
}

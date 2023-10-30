using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Employees.HolidayType;

namespace Dawem.Models.AutoMapper
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

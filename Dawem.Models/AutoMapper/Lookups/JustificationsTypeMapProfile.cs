using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Lookups;
using Dawem.Models.Dtos.Core.JustificationsTypes;
using Dawem.Models.Dtos.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawem.Models.AutoMapper.Lookups
{
    public class JustificationsTypeMapProfile : Profile
    {
        public JustificationsTypeMapProfile()
        {
            CreateMap<CreateJustificationsTypeDTO, JustificationsType>();
            CreateMap<UpdateJustificationsTypeDTO, JustificationsType>();
        }
    }
}

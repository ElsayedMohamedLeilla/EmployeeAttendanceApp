using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Dawem.Employees.JobTitles;

namespace Dawem.Models.AutoMapper.Dawem
{
    public class JobTitlesMapProfile : Profile
    {
        public JobTitlesMapProfile()
        {
            CreateMap<CreateJobTitleModel, JobTitle>();
            CreateMap<UpdateJobTitleModel, JobTitle>();
        }
    }
}

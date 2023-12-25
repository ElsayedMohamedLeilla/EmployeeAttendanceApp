using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Employees.JobTitles;

namespace Dawem.Models.AutoMapper
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

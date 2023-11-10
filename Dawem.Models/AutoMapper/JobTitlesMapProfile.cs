using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Employees.JobTitle;

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

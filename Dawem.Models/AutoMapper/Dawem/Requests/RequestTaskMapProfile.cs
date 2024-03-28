using AutoMapper;
using Dawem.Domain.Entities.Requests;
using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Requests.Tasks;

namespace Dawem.Models.AutoMapper.Dawem.Requests
{
    public class RequestTaskMapProfile : Profile
    {
        public RequestTaskMapProfile()
        {
            CreateMap<CreateRequestTaskModelDTO, Request>()
                 .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.DateFrom))
                 .ForMember(dest => dest.Type, opts => opts.MapFrom(src => RequestType.Task))
                .AfterMap(MapRequestTask);

            CreateMap<CreateRequestTaskModelDTO, RequestTask>();

            CreateMap<UpdateRequestTaskModelDTO, Request>()
                .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.DateFrom))
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => RequestType.Task))
                .AfterMap(MapRequestTask);
        }
        private void MapRequestTask(CreateRequestTaskModelDTO source, Request destination, ResolutionContext context)
        {
            destination.RequestTask = context.Mapper.Map<RequestTask>(source);
            destination.RequestTask.TaskEmployees = source.TaskEmployeeIds
                .Select(employeeId => new RequestTaskEmployee { EmployeeId = employeeId })
                .ToList();

            if (source.AttachmentsNames != null && source.AttachmentsNames.Count > 0)
            {
                destination.RequestAttachments = source.AttachmentsNames
                .Select(fileName => new RequestAttachment { FileName = fileName })
                .ToList();
            }
        }
        private void MapRequestTask(UpdateRequestTaskModelDTO source, Request destination, ResolutionContext context)
        {
            destination.RequestTask = context.Mapper.Map<RequestTask>(source);
            destination.RequestTask.TaskEmployees = source.TaskEmployeeIds
                .Select(employeeId => new RequestTaskEmployee { EmployeeId = employeeId })
                .ToList();

            if (source.AttachmentsNames != null && source.AttachmentsNames.Count > 0)
            {
                destination.RequestAttachments = source.AttachmentsNames
                .Select(fileName => new RequestAttachment { FileName = fileName })
                .ToList();
            }
        }
    }
}

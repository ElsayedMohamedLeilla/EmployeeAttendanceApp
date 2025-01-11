using AutoMapper;
using Dawem.Domain.Entities.Requests;
using Dawem.Enums.Generals;
using Dawem.Models.Requests.Justifications;

namespace Dawem.Models.AutoMapper.Dawem.Requests
{
    public class RequestOvertimeMapProfile : Profile
    {
        public RequestOvertimeMapProfile()
        {
            CreateMap<CreateRequestOvertimeDTO, Request>()
                 .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.OvertimeDate))
                 .ForMember(dest => dest.Type, opts => opts.MapFrom(src => RequestType.Overtime))
                .AfterMap(MapRequestOvertime);

            CreateMap<CreateRequestOvertimeDTO, RequestOvertime>();

            CreateMap<UpdateRequestOvertimeDTO, Request>()
                .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.OvertimeDate))
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => RequestType.Overtime))
                .AfterMap(MapRequestOvertime);
        }
        private void MapRequestOvertime(CreateRequestOvertimeDTO source, Request destination, ResolutionContext context)
        {
            destination.RequestOvertime = context.Mapper.Map<RequestOvertime>(source);


            if (source.AttachmentsNames != null && source.AttachmentsNames.Count > 0)
            {
                destination.RequestAttachments = source.AttachmentsNames
                .Select(fileName => new RequestAttachment { FileName = fileName })
                .ToList();
            }
        }
        private void MapRequestOvertime(UpdateRequestOvertimeDTO source, Request destination, ResolutionContext context)
        {
            destination.RequestOvertime = context.Mapper.Map<RequestOvertime>(source);


            if (source.AttachmentsNames != null && source.AttachmentsNames.Count > 0)
            {
                destination.RequestAttachments = source.AttachmentsNames
                .Select(fileName => new RequestAttachment { FileName = fileName })
                .ToList();
            }
        }
    }
}

using AutoMapper;
using Dawem.Domain.Entities.Requests;
using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Requests.Justifications;

namespace Dawem.Models.AutoMapper.Dawem.Requests
{
    public class RequestJustificationMapProfile : Profile
    {
        public RequestJustificationMapProfile()
        {
            CreateMap<CreateRequestJustificationDTO, Request>()
                 .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.DateFrom))
                 .ForMember(dest => dest.Type, opts => opts.MapFrom(src => RequestType.Justification))
                .AfterMap(MapRequestJustification);

            CreateMap<CreateRequestJustificationDTO, RequestJustification>();

            CreateMap<UpdateRequestJustificationDTO, Request>()
                .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.DateFrom))
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => RequestType.Justification))
                .AfterMap(MapRequestJustification);
        }
        private void MapRequestJustification(CreateRequestJustificationDTO source, Request destination, ResolutionContext context)
        {
            destination.RequestJustification = context.Mapper.Map<RequestJustification>(source);


            if (source.AttachmentsNames != null && source.AttachmentsNames.Count > 0)
            {
                destination.RequestAttachments = source.AttachmentsNames
                .Select(fileName => new RequestAttachment { FileName = fileName })
                .ToList();
            }
        }
        private void MapRequestJustification(UpdateRequestJustificationDTO source, Request destination, ResolutionContext context)
        {
            destination.RequestJustification = context.Mapper.Map<RequestJustification>(source);


            if (source.AttachmentsNames != null && source.AttachmentsNames.Count > 0)
            {
                destination.RequestAttachments = source.AttachmentsNames
                .Select(fileName => new RequestAttachment { FileName = fileName })
                .ToList();
            }
        }
    }
}

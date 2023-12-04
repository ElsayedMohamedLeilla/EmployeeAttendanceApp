using AutoMapper;
using Dawem.Domain.Entities.Requests;
using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Requests.Permissions;

namespace Dawem.Models.AutoMapper.Requests
{
    public class RequestPermissionMapProfile : Profile
    {
        public RequestPermissionMapProfile()
        {
            CreateMap<CreateRequestPermissionModelDTO, Request>()
                 .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.DateFrom))
                 .ForMember(dest => dest.Type, opts => opts.MapFrom(src => RequestType.Permissions))
                .AfterMap(MapRequestPermission);

            CreateMap<CreateRequestPermissionModelDTO, RequestPermission>();

            CreateMap<UpdateRequestPermissionModelDTO, Request>()
                .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.DateFrom))
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => RequestType.Permissions))
                .AfterMap(MapRequestPermission);
        }
        private void MapRequestPermission(CreateRequestPermissionModelDTO source, Request destination, ResolutionContext context)
        {
            destination.RequestPermission = context.Mapper.Map<RequestPermission>(source);

            if (source.AttachmentsNames != null && source.AttachmentsNames.Count > 0)
            {
                destination.RequestAttachments = source.AttachmentsNames
                .Select(fileName => new RequestAttachment { FileName = fileName })
                .ToList();
            }
        }
        private void MapRequestPermission(UpdateRequestPermissionModelDTO source, Request destination, ResolutionContext context)
        {
            destination.RequestPermission = context.Mapper.Map<RequestPermission>(source);

            if (source.AttachmentsNames != null && source.AttachmentsNames.Count > 0)
            {
                destination.RequestAttachments = source.AttachmentsNames
                .Select(fileName => new RequestAttachment { FileName = fileName })
                .ToList();
            }
        }
    }
}

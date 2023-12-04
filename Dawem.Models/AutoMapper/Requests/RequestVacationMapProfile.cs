using AutoMapper;
using Dawem.Domain.Entities.Requests;
using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Requests.Vacations;

namespace Dawem.Models.AutoMapper.Requests
{
    public class RequestVacationMapProfile : Profile
    {
        public RequestVacationMapProfile()
        {
            CreateMap<CreateRequestVacationDTO, Request>()
                 .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.DateFrom))
                 .ForMember(dest => dest.Type, opts => opts.MapFrom(src => RequestType.Vacations))
                .AfterMap(MapRequestVacation);

            CreateMap<CreateRequestVacationDTO, RequestVacation>();

            CreateMap<UpdateRequestVacationDTO, Request>()
                .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.DateFrom))
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => RequestType.Vacations))
                .AfterMap(MapRequestVacation);
        }
        private void MapRequestVacation(CreateRequestVacationDTO source, Request destination, ResolutionContext context)
        {
            destination.RequestVacation = context.Mapper.Map<RequestVacation>(source);


            if (source.AttachmentsNames != null && source.AttachmentsNames.Count > 0)
            {
                destination.RequestAttachments = source.AttachmentsNames
                .Select(fileName => new RequestAttachment { FileName = fileName })
                .ToList();
            }
        }
        private void MapRequestVacation(UpdateRequestVacationDTO source, Request destination, ResolutionContext context)
        {
            destination.RequestVacation = context.Mapper.Map<RequestVacation>(source);


            if (source.AttachmentsNames != null && source.AttachmentsNames.Count > 0)
            {
                destination.RequestAttachments = source.AttachmentsNames
                .Select(fileName => new RequestAttachment { FileName = fileName })
                .ToList();
            }
        }
    }
}

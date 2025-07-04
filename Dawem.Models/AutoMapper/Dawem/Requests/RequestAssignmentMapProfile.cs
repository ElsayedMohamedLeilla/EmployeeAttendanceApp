﻿using AutoMapper;
using Dawem.Domain.Entities.Requests;
using Dawem.Enums.Generals;
using Dawem.Models.Requests.Assignments;

namespace Dawem.Models.AutoMapper.Dawem.Requests
{
    public class RequestAssignmentMapProfile : Profile
    {
        public RequestAssignmentMapProfile()
        {
            CreateMap<CreateRequestAssignmentModelDTO, Request>()
                 .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.DateFrom))
                 .ForMember(dest => dest.Type, opts => opts.MapFrom(src => RequestType.Assignment))
                .AfterMap(MapRequestAssignment);

            CreateMap<CreateRequestAssignmentModelDTO, RequestAssignment>();

            CreateMap<UpdateRequestAssignmentModelDTO, Request>()
                .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.DateFrom))
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => RequestType.Assignment))
                .AfterMap(MapRequestAssignment);
        }
        private void MapRequestAssignment(CreateRequestAssignmentModelDTO source, Request destination, ResolutionContext context)
        {
            destination.RequestAssignment = context.Mapper.Map<RequestAssignment>(source);

            if (source.AttachmentsNames != null && source.AttachmentsNames.Count > 0)
            {
                destination.RequestAttachments = source.AttachmentsNames
                .Select(fileName => new RequestAttachment { FileName = fileName })
                .ToList();
            }
        }
        private void MapRequestAssignment(UpdateRequestAssignmentModelDTO source, Request destination, ResolutionContext context)
        {
            destination.RequestAssignment = context.Mapper.Map<RequestAssignment>(source);

            if (source.AttachmentsNames != null && source.AttachmentsNames.Count > 0)
            {
                destination.RequestAttachments = source.AttachmentsNames
                .Select(fileName => new RequestAttachment { FileName = fileName })
                .ToList();
            }
        }
    }
}

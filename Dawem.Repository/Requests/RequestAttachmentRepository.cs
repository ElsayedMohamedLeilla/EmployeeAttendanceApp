﻿using Dawem.Contract.Repository.Requests;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.DTOs.Dawem.Generic;

namespace Dawem.Repository.Requests
{
    public class RequestAttachmentRepository : GenericRepository<RequestAttachment>, IRequestAttachmentRepository
    {
        public RequestAttachmentRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }
}

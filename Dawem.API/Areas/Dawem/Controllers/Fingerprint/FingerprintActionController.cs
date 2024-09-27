using Dawem.Contract.BusinessLogic.Dawem.Attendances;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Dtos.Dawem.Fingerprint;
using Dawem.Models.DTOs.Dawem.Generic;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace Dawem.API.Areas.Dawem.Controllers.Schedules
{
    [ApiController]
    [Route(LeillaKeys.EmptyString)]
    public class FingerprintActionController : DawemControllerBase
    {
        private readonly IFingerprintActionBL fingerprintActionBL;
        private readonly IRepositoryManager repositoryManager;
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        public FingerprintActionController(IFingerprintActionBL _fingerprintActionBL,
            IRepositoryManager _repositoryManager,
            IUnitOfWork<ApplicationDBContext> _unitOfWork)
        {
            fingerprintActionBL = _fingerprintActionBL;
            unitOfWork = _unitOfWork;
            repositoryManager = _repositoryManager;
        }
        [HttpPost]
        [Route(LeillaKeys.ReadFingerprintRoute)]
        public async Task<ActionResult> ReadFingerprint([FromQuery] ReadFingerprintModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest();
                }

                if (model.Table != "ATTLOG")
                    return Ok("Ok");

                var result = await fingerprintActionBL.ReadFingerprint(model);

                if (result)
                {
                    return Ok("Ok");
                }
                else
                {
                    return UnprocessableEntity();
                }
            }
            catch (Exception ex)
            {
                #region Insert Exception

                var getNextCodes = await repositoryManager.FingerprintDeviceRepository
                       .Get(e => e.CompanyId == 7)
                       .Select(e => e.Code)
                       .DefaultIfEmpty()
                       .MaxAsync() + 1;
                repositoryManager.FingerprintDeviceRepository.Insert(new FingerprintDevice
                {
                    Name = model.Table + DateTime.UtcNow,
                    Code = getNextCodes,
                    Notes = "Data:" + "###" + ex.Message,
                    AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.Now,
                    CompanyId = 7,
                    SerialNumber = "Hatch"
                });
                await unitOfWork.SaveAsync();

                #endregion
            }

            return BadRequest();
        }
        [HttpPost]
        [Route("iclock/devicecmd")]
        public async Task<ActionResult> Test1([FromQuery] ReadFingerprintModel query)
        {
            return Ok("Ok");
        }
        [HttpGet]
        [Route("iclock/ping")]
        public async Task<ActionResult> Test2([FromQuery] ReadFingerprintModel query)
        {
            return Ok("Ok");
        }
        [HttpGet]
        [Route("iclock/getrequest")]
        public async Task<ActionResult> Test3([FromQuery] ReadFingerprintModel query)
        {
            return Ok("Ok");
        }
        [HttpGet]
        [Route("iclock/cdata")]
        public async Task<ActionResult> Test4([FromQuery] ReadFingerprintModel query)
        {
            return Ok("Ok");
        }
    }
}
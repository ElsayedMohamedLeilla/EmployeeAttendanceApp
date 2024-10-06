using Dawem.Contract.BusinessLogic.Dawem.Attendances;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Models.Dtos.Dawem.Fingerprint;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult> ReadFingerprint([FromQuery] ReadFingerprintControllerModel controllerModel)
        {
            if (controllerModel == null)
            {
                return BadRequest();
            }

            #region Map Model

            var model = new ReadFingerprintModel
            {
                SN = controllerModel.SN,
                Table = controllerModel.Table,
                INFO = controllerModel.INFO,
                Stamp = controllerModel.Stamp,
                Options = controllerModel.Options,
                Pushver = controllerModel.Pushver,
                Language = controllerModel.Language,
                PushCommkey = controllerModel.PushCommkey
            };

            #endregion

            try
            {
                #region Set Request Body

                using StreamReader reader = new(Request.Body, Encoding.UTF8);
                var bodyString = await reader.ReadToEndAsync();

                model.RequestBody = Request.Body;
                model.RequestBodyString = bodyString;

                #endregion

                model.LogType = LeillaKeys.Log;
                await fingerprintActionBL.AddFingerprintLog(model);

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
                model.Exception = ex;
                model.LogType = LeillaKeys.Exception;
                await fingerprintActionBL.AddFingerprintLog(model);
            }

            return BadRequest();
        }
        [HttpPost]
        [Route("iclock/devicecmd")]
        public async Task<ActionResult> Test1([FromQuery] ReadFingerprintControllerModel query)
        {
            return Ok("Ok");
        }
        [HttpGet]
        [Route("iclock/ping")]
        public async Task<ActionResult> Test2([FromQuery] ReadFingerprintControllerModel query)
        {
            return Ok("Ok");
        }
        [HttpGet]
        [Route("iclock/getrequest")]
        public async Task<ActionResult> Test3([FromQuery] ReadFingerprintControllerModel query)
        {
            return Ok("Ok");
        }
        [HttpGet]
        [Route("iclock/cdata")]
        public async Task<ActionResult> Test4([FromQuery] ReadFingerprintControllerModel query)
        {
            return Ok("Ok");
        }
    }
}
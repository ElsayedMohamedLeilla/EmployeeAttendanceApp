using Dawem.Contract.BusinessLogic.Dawem.Provider;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Providers.Companies;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dawem.API.Areas.Dawem.Controllers.Provider
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize]
    public class CompanyController : DawemControllerBase
    {
        private readonly ICompanyBL companyBL;


        public CompanyController(ICompanyBL _companyBL)
        {
            companyBL = _companyBL;
        }
        [HttpPut, DisableRequestSizeLimit]
        public async Task<ActionResult> Update([FromForm] UpdateCompanyWithFilesModel formData)
        {
            if (formData == null || formData.UpdateCompanyModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<UpdateCompanyModel>(formData.UpdateCompanyModelString);
            model.LogoImageFile = formData.LogoImageFile;
            model.Attachments = formData.Attachments;
            var result = await companyBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateCompanySuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int companyId)
        {
            if (companyId < 1)
            {
                return BadRequest();
            }
            return Success(await companyBL.GetById(companyId));
        }
    }
}
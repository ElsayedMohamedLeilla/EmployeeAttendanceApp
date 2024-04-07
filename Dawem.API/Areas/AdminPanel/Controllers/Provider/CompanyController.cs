using Dawem.API.Areas.Dawem.Controllers;
using Dawem.Contract.BusinessLogic.Dawem.Provider;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Providers.Companies;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dawem.API.Areas.AdminPanel.Controllers.Provider
{
    [Route(LeillaKeys.AdminPanelApiControllerAction), ApiController, Authorize, AdminPanelAuthorize]
    public class CompanyController : AdminPanelControllerBase
    {
        private readonly ICompanyBL companyBL;

        public CompanyController(ICompanyBL _companyBL)
        {
            companyBL = _companyBL;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> Create([FromForm] CreateCompanyWithFilesModel formData)
        {
            if (formData == null || formData.CreateCompanyModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<CreateCompanyModel>(formData.CreateCompanyModelString);
            model.LogoImageFile = formData.LogoImageFile;
            model.Attachments = formData.Attachments;
            var result = await companyBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateCompanySuccessfully);
        }

        [HttpPut, DisableRequestSizeLimit]
        public async Task<ActionResult> Update([FromForm] UpdateCompanyWithFilesModel formData)
        {
            if (formData == null || formData.UpdateCompanyModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<AdminPanelUpdateCompanyModel>(formData.UpdateCompanyModelString);
            model.LogoImageFile = formData.LogoImageFile;
            model.Attachments = formData.Attachments;
            var result = await companyBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateCompanySuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetCompaniesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var companiesresponse = await companyBL.Get(criteria);
            return Success(companiesresponse.Companies, companiesresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetCompaniesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var companiesresponse = await companyBL.GetForDropDown(criteria);

            return Success(companiesresponse.Companies, companiesresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int companyId)
        {
            if (companyId < 1)
            {
                return BadRequest();
            }
            return Success(await companyBL.GetInfo(companyId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById()
        {
            return Success(await companyBL.GetById());
        }
        [HttpPut]
        public async Task<ActionResult> Enable(int companyId)
        {
            if (companyId < 1)
            {
                return BadRequest();
            }
            return Success(await companyBL.Enable(companyId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await companyBL.Disable(model));
        }
        [HttpDelete]
        
        public async Task<ActionResult> Delete(int companyId)
        {
            if (companyId < 1)
            {
                return BadRequest();
            }
            return Success(await companyBL.Delete(companyId));
        }
        [HttpGet]
        public async Task<ActionResult> GetCompaniesInformations()
        {
            return Success(await companyBL.GetCompaniesInformations());
        }
    }
}
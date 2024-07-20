using Dawem.Contract.BusinessLogic.Dawem.Reports;
using Dawem.Enums.Generals;
using Dawem.Models.Response.Dawem.ReportCritrias;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Reports
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]
    public class ReportController : DawemControllerBase
    {

        private readonly IReportGeneratorBL _reportGeneratorBL;
        public ReportController(IReportGeneratorBL reportGeneratorBL)
        {
            _reportGeneratorBL = reportGeneratorBL;
        }

        #region Attendance Report
       
       
       
       
       
       
        
     
       
        #endregion


        #region Summons Report
      

       

       
        #endregion

        #region Statistics Report
     

      

       
        #endregion



    }
}

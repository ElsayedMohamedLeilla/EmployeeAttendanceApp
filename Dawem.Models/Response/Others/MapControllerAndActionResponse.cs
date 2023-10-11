using SmartBusinessERP.Enums;

namespace SmartBusinessERP.Models.Response.Others
{
    public class MapControllerAndActionResponse : BaseResponse
    {
        public ERPScreen? Screen { get; set; }
        public ApiMethod? Method { get; set; }
    }
}

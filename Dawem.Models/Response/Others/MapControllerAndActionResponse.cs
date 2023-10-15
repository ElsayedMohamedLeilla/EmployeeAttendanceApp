using Dawem.Enums.General;

namespace Dawem.Models.Response.Others
{
    public class MapControllerAndActionResponse : BaseResponse
    {
        public ApplicationScreenType? Screen { get; set; }
        public ApiMethod? Method { get; set; }
    }
}

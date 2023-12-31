using Dawem.Enums.Permissions;

namespace Dawem.Models.Response.Others
{
    public class MapControllerAndActionResponse
    {
        public ApplicationScreenCode? Screen { get; set; }
        public ApplicationAction? Method { get; set; }
    }
}

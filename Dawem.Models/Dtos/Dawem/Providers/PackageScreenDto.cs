using Dawem.Models.Dtos.Dawem.Lookups;

namespace Dawem.Models.Dtos.Dawem.Providers
{
    public class PackageScreenDto
    {
        int BranchId { get; set; }
        public int PageId { get; set; }

        public virtual ScreenDto ScreenModule { get; set; }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
    }
}

namespace Dawem.Models.Dtos.Provider
{
    public class PackageDto
    {

        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }

        public int NumberOfBranches { get; set; }

        public int NumberOfUsers { get; set; }


        public bool IsDefaultPackage { get; set; }



        public bool IsActive { get; set; }

        public virtual List<PackageScreenDto> PackageScreenModules { get; set; }
    }
}


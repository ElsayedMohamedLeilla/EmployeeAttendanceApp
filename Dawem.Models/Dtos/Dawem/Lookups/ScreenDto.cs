﻿using Dawem.Enums.Permissions;
using Dawem.Models.Dtos.Dawem.Providers;

namespace Dawem.Models.Dtos.Dawem.Lookups
{
    public class ScreenDto
    {
        public string Description { get; set; }
        public string DescriptionAr { get; set; }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }

        public string Icon { get; set; }
        public string LinkId { get; set; }


        public int? ParentId { get; set; }
        public DawemAdminApplicationScreenCode Parent { get; set; }
        public int Level { get; set; }

        public int Order { get; set; }


        public List<ScreenDto> ScreenModules { get; set; }
        public virtual List<PackageScreenDto> PackageScreens { get; set; }



        public bool IsActive { get; set; }
    }
}

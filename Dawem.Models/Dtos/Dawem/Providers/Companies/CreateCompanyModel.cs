﻿namespace Dawem.Models.Dtos.Dawem.Providers.Companies
{
    public class CreateCompanyModel : BaseCompanyModel
    {
        public string Name { get; set; }
        public int NumberOfEmployees { get; set; }
    }
}
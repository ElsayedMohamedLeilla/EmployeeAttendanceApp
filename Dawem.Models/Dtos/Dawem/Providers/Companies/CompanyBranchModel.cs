﻿namespace Dawem.Models.Dtos.Dawem.Providers.Companies
{
    public class CompanyBranchModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string PostalCode { get; set; }
    }
}

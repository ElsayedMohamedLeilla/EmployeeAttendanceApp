﻿namespace Dawem.Models.Response.Dawem.Core.Responsibilities
{
    public class GetResponsibilityByIdResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool ForEmployeesApplication { get; set; }
        public bool IsActive { get; set; }
    }
}

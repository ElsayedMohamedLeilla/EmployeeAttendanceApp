﻿namespace Dawem.Models.Response.Dawem.Employees.TaskTypes
{
    public class GetTaskTypesResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}

﻿using Dawem.Models.Dtos.Dawem.Employees.Departments;

namespace Dawem.Models.Response.Dawem.Employees.Departments
{
    public class GetDepartmentsResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int NumberOfEmployees { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public DepartmentManagarForGridDTO Manager { get; set; } = null;

    }
}

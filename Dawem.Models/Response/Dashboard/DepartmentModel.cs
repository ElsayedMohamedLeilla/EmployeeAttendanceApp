namespace Dawem.Models.Response.Employees.Employee
{
    public class DepartmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EmployeesCount { get; set; }
        public DateTime LastEditDate { get; set; }
        public List<DepartmentEmployeeModel> Employees { get; set; }
    }
}

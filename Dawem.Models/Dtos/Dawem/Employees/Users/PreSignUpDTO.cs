﻿namespace Dawem.Models.Dtos.Dawem.Employees.Users
{
    public class PreSignUpDTO
    {
        public string CompanyVerificationCode { get; set; }
        public int EmployeeNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }


    }
}

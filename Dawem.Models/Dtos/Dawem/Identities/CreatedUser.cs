﻿using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Identities
{
    public class CreatedUser
    {
        public int Id { get; set; }
        public virtual string UserName { get; set; }
        public int MainBranchId { get; set; }
        public bool IsActive { get; set; }
        public int MobileCountryId { get; set; }
        public string MobileNumber { get; set; }
        public virtual string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public List<string> UserRols { get; set; }
        public List<UserBranch> UserBranches { get; set; }
    }


}

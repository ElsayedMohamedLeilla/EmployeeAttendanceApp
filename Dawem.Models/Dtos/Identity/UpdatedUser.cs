namespace Dawem.Models.Dtos.Identity
{
    public class UpdatedUser
    {
        public int Id { get; set; }

        public virtual string? UserName { get; set; }
        public bool IsActive { get; set; }
        public string? MobileNumber { get; set; }
        public virtual string? Email { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public virtual string? PhoneNumber { get; set; }
        public virtual string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public List<string> UserRols { get; set; }

        public List<int> userBranches { get; set; }

        public List<int> UserGroups { get; set; }
        public int? MainBranchId { get; set; }
    }


}

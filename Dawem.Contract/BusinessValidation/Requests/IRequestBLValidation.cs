namespace Dawem.Contract.BusinessValidation.Requests
{
    public interface IRequestBLValidation
    {
        Task<bool> IsEmployeeValidation();
    }
}

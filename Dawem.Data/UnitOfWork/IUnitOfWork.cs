using Microsoft.EntityFrameworkCore;

namespace Dawem.Data.UnitOfWork
{
    public interface IUnitOfWork<out TContext> where TContext : DbContext, new()
    {
        ApplicationDBContext Context { get; }
        void CreateTransaction();
        void Commit();
        void Rollback();

        Task CreateTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        void Save();
        Task SaveAsync();
        void DetachEntity<T>(T Entity) where T : class;

    }
}
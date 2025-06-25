namespace UsersMS.Core.Infrastructure.DB
{
    public interface IDbContextTransactionProxy : IDisposable
    {
        void Commit();
        void Rollback();
    }
}

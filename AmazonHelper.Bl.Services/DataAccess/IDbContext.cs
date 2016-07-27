namespace AmazonHelper.DataAccess
{
    using System;
    using System.Threading.Tasks;

    public interface IDbContext : IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}

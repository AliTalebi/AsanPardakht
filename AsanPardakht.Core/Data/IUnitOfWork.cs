namespace AsanPardakht.Core.Data
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default!);
    }
}

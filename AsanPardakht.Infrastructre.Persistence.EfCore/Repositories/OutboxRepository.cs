using AsanPardakht.Core.Data;

namespace AsanPardakht.Infrastructre.Persistence.EfCore.Repositories
{
    public sealed class OutboxRepository : IOutboxRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public OutboxRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public void Insert(OutBoxEventData eventData)
        {
            _applicationDbContext.Add(eventData);
        }
    }
}

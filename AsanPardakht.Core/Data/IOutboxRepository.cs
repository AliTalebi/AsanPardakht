namespace AsanPardakht.Core.Data
{
    public interface IOutboxRepository : IRepository
    {
        void Insert(OutBoxEventData eventData);
    }
}

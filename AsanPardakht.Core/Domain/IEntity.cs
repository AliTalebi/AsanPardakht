namespace AsanPardakht.Core.Domain
{
    public interface IEntity<TEntityId> where TEntityId : notnull
    {
        public TEntityId Id { get; }
    }
}

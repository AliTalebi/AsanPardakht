using AsanPardakht.Core.Common;
using AsanPardakht.Core.Data;
using AsanPardakht.Core.Domain.Events;
using AsanPardakht.Core.OutboxEvents.People;
using AsanPardakht.Core.Security;
using AsanPardakht.Domain.People;
using AsanPardakht.Domain.People.Events;

namespace AsanPardakht.Application.EventHandlers
{
    public class PersonAddressAddedEventHandler : IDomainEventHandler<PersonAddressAdded>
    {
        private readonly IOutboxRepository _outboxRepository;
        private readonly IUserIdentityAccessor _userIdentityAccessor;
        public PersonAddressAddedEventHandler(IOutboxRepository outboxRepository, IUserIdentityAccessor userIdentityAccessor)
        {
            _outboxRepository = outboxRepository;
            _userIdentityAccessor = userIdentityAccessor;
        }

        public async Task ExecuteAsync(PersonAddressAdded domainEvent, CancellationToken cancellationToken = default)
        {
            PersonAddressAddedOutbox personAddressAddedOutbox = new()
            {
                Detail = domainEvent.Detail,
                Id = domainEvent.Id.ToString(),
                CityName = domainEvent.CityName,
                ProvinceName = domainEvent.ProvinceName
            };

            var outBoxEventData = await Task.FromResult(personAddressAddedOutbox.ToOutboxEventData(_userIdentityAccessor.GetUserName(), DateTime.Now, typeof(Person).GetType().AssemblyQualifiedName));

            _outboxRepository.Insert(outBoxEventData);
        }
    }
}

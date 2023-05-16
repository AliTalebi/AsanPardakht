using AsanPardakht.Core.Data;
using AsanPardakht.Core.Common;
using Microsoft.EntityFrameworkCore;
using AsanPardakht.Core.OutboxEvents;
using AsanPardakht.Core.OutboxEvents.People;
using AsanPardakht.WorkerService.MessageProcessorConsoleApp.Data;
using AsanPardakht.WorkerService.MessageProcessorConsoleApp.Data.Entities;

namespace AsanPardakht.WorkerService.MessageProcessorConsoleApp.Outbox
{
    public sealed class ReadOutboxSqlServerEventTask : ReadOutboxBackgroundTask
    {
        private OutBoxEventDbContext? _outBoxEventDbContext;
        private ProcessDataDbContext? _processDataDbContext;
        private readonly DbContextOptions<OutBoxEventDbContext> _outBoxEventDbContextOptions;
        private readonly DbContextOptions<ProcessDataDbContext> _processDataDbContextOptions;

        public ReadOutboxSqlServerEventTask(DbContextOptions<ProcessDataDbContext> processDataDbContextOptions, DbContextOptions<OutBoxEventDbContext> outBoxEventDbContextOptions)
        {
            _outBoxEventDbContextOptions = outBoxEventDbContextOptions;
            _processDataDbContextOptions = processDataDbContextOptions;
        }

        protected override void OnPrepare()
        {
            _outBoxEventDbContext = new(_outBoxEventDbContextOptions);
            _processDataDbContext = new(_processDataDbContextOptions);

            _processDataDbContext.Database.EnsureCreated();
        }

        protected override async Task OnExecuteAsync(CancellationToken cancellationToken)
        {
            bool hasData = true;
            long currentCursur = 1;

            while (hasData)
            {
                var events = await _outBoxEventDbContext!.Set<OutBoxEventData>().Where(x => x.Read == false).OrderBy(x => x.Id).Take(20).ToListAsync(cancellationToken);

                if (events == null || events.Count == 0)
                {
                    hasData = false;
                    continue;
                }

                currentCursur = events.Last().Id;

                List<ProcessedTehranCityAddress> processedTehranCityAddresses = new();

                foreach (var item in events)
                {
                    var outBoxEventType = Type.GetType(item.EventType);

                    if (outBoxEventType != null)
                    {
                        IOutboxEvent? outboxEvent = item.Data.Deserialize(outBoxEventType) as IOutboxEvent;

                        if (outboxEvent != null)
                        {
                            switch (outboxEvent)
                            {
                                case PersonAddressAddedOutbox personAddressAddedOutbox:
                                    {
                                        if (personAddressAddedOutbox.CityName.Equals("تهران") || personAddressAddedOutbox.CityName.ToLower().Equals("tehran"))
                                        {
                                            processedTehranCityAddresses.Add(new ProcessedTehranCityAddress()
                                            {
                                                Address = personAddressAddedOutbox.Detail,
                                                CityName = personAddressAddedOutbox.CityName,
                                                ProvinceName = personAddressAddedOutbox.ProvinceName
                                            });

                                            item.Read = true;
                                            item.ReadAt = DateTime.Now;

                                        }

                                        break;
                                    }
                            }
                        }
                    }
                }

                if (processedTehranCityAddresses != null && processedTehranCityAddresses.Count > 0)
                {
                    //we can use bulkinsert
                    await _processDataDbContext!.AddRangeAsync(processedTehranCityAddresses, cancellationToken);
                    await _processDataDbContext.SaveChangesAsync(cancellationToken);

                    await _outBoxEventDbContext.SaveChangesAsync(cancellationToken);
                }

                events = null;
            }
        }

        protected override void OnDestroying()
        {
            _outBoxEventDbContext?.Dispose();
            _outBoxEventDbContext = null;

            _processDataDbContext?.Dispose();
            _processDataDbContext = null;
        }
    }
}

using Microsoft.Extensions.Hosting;
using AsanPardakht.WorkerService.MessageProcessorConsoleApp.Outbox;

namespace AsanPardakht.WorkerService.MessageProcessorConsoleApp
{
    public class SyncDataWorkerService : BackgroundService
    {
        private readonly IReadOutboxEventTask _outboxEventHandler;
        public SyncDataWorkerService(IReadOutboxEventTask outboxEventHandler)
        {
            _outboxEventHandler = outboxEventHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _outboxEventHandler.StartAsync();
        }
    }
}

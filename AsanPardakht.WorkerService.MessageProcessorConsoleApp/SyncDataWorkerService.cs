using Microsoft.Extensions.Hosting;
using AsanPardakht.WorkerService.MessageProcessorConsoleApp.Outbox;
using Microsoft.Extensions.Logging;

namespace AsanPardakht.WorkerService.MessageProcessorConsoleApp
{
    public class SyncDataWorkerService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IReadOutboxEventTask _outboxEventHandler;
        public SyncDataWorkerService(IReadOutboxEventTask outboxEventHandler,ILogger<SyncDataWorkerService> logger)
        {
            _logger = logger;
            _outboxEventHandler = outboxEventHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
				await _outboxEventHandler.StartAsync();
			}
            catch (Exception ex)
            {
                _logger.LogError(ex, "exception occurred when calling Startasync method ");
            }
        }
    }
}

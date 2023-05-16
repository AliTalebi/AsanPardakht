namespace AsanPardakht.WorkerService.MessageProcessorConsoleApp.Outbox
{
    public interface IReadOutboxEventTask
    {
        Task StartAsync();
        Task StopAsync();
    }
}

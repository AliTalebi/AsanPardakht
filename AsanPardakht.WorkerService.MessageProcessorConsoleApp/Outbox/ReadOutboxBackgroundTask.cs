namespace AsanPardakht.WorkerService.MessageProcessorConsoleApp.Outbox
{
    public abstract class ReadOutboxBackgroundTask : IReadOutboxEventTask
    {
        private PeriodicTimer? _timer;
        private Task? _timerTask = null;
        private CancellationTokenSource _cancellationTokenSource = new();

        public Task StartAsync()
        {
            _timer = new(TimeSpan.FromMilliseconds(5000));

            OnPrepare();

            _timerTask = ExecuteAsnc();

            return Task.CompletedTask;
        }

        public async Task StopAsync()
        {
            if (_timerTask == null)
            {
                return;
            }

            OnDestroying();

            _cancellationTokenSource.Cancel();

            await _timerTask;

            _cancellationTokenSource.Dispose();

        }

        private async Task ExecuteAsnc()
        {
            try
            {
                while (await _timer!.WaitForNextTickAsync(_cancellationTokenSource.Token))
                {
                    await OnExecuteAsync(_cancellationTokenSource.Token);
                }
            }
            catch (OperationCanceledException ex)
            {
            }
        }

        protected abstract void OnPrepare();
        protected abstract void OnDestroying();
        protected abstract Task OnExecuteAsync(CancellationToken cancellationToken);
    }
}

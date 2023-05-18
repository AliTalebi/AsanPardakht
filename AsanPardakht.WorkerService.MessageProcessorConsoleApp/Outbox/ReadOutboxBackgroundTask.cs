using Microsoft.Extensions.Logging;

namespace AsanPardakht.WorkerService.MessageProcessorConsoleApp.Outbox
{
	public abstract class ReadOutboxBackgroundTask : IReadOutboxEventTask
	{
		private PeriodicTimer? _timer;
		private Task? _timerTask = null;
		protected readonly ILogger Logger;
		private CancellationTokenSource _cancellationTokenSource = new();

		protected ReadOutboxBackgroundTask(ILogger logger)
		{
			Logger = logger;
		}

		public Task StartAsync()
		{
			_timer = new(TimeSpan.FromMinutes(10));

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
				Logger.LogError(ex, "ExecuteAsync method was cancelled");
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "unhanled exception occurred in ExecuteAsync method");
			}
		}

		protected abstract void OnPrepare();
		protected abstract void OnDestroying();
		protected abstract Task OnExecuteAsync(CancellationToken cancellationToken);
	}
}

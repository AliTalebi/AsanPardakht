using System.Text;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using AsanPardakht.WorkerService.MessageProcessorConsoleApp.Data;
using AsanPardakht.WorkerService.MessageProcessorConsoleApp.Options;
using AsanPardakht.WorkerService.MessageProcessorConsoleApp.Data.Entities;
using AsanPardakht.WorkerService.MessageProcessorConsoleApp.Outbox.Http.DTO;
using Microsoft.Extensions.Logging;

namespace AsanPardakht.WorkerService.MessageProcessorConsoleApp.Outbox.Http
{
	public sealed class ReadOutboxHttpRequestTask : ReadOutboxBackgroundTask
	{
		private ProcessDataDbContext? _processDataDbContext;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IOptionsMonitor<ApiOptions> _apiOptions;
		private AuthenticationHeaderValue? _basicAuthorizationHeader = null;
		private readonly DbContextOptions<ProcessDataDbContext> _processDataDbContextOptions;

		public ReadOutboxHttpRequestTask(ILogger<ReadOutboxHttpRequestTask> logger, IHttpClientFactory httpClientFactory, IOptionsMonitor<ApiOptions> optionsMonitorApi, DbContextOptions<ProcessDataDbContext> processDataDbContextOptions)
			: base(logger)
		{
			_apiOptions = optionsMonitorApi;
			_httpClientFactory = httpClientFactory;
			_processDataDbContextOptions = processDataDbContextOptions;
		}

		protected override void OnPrepare()
		{
			_processDataDbContext = new(_processDataDbContextOptions);

			var userNameCredential = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_apiOptions.CurrentValue.Credential));
			_basicAuthorizationHeader = new AuthenticationHeaderValue("Basic", userNameCredential);
		}

		protected override async Task OnExecuteAsync(CancellationToken cancellationToken)
		{
			int page = 1;
			int pageSize = 50;
			bool hasData = true;
			int exceptionCount = 0;
			StringBuilder queryStringBuilder = new();

			while (hasData && exceptionCount <= 5)
			{
				Console.WriteLine("reading from web strated");

				queryStringBuilder.Clear();
				HttpClient httpClient = _httpClientFactory.CreateClient();
				httpClient.BaseAddress = new(_apiOptions.CurrentValue.BaseAddress);
				httpClient.DefaultRequestHeaders.Authorization = _basicAuthorizationHeader;

				queryStringBuilder.Append(_apiOptions.CurrentValue.Api).Append('?').Append("page").Append('=').Append(page)
					.Append('&').Append("pagesize").Append('=').Append(pageSize)
					.Append('&').Append("cityname").Append('=').Append("تهران");

				HttpRequestMessage getPeopleAddressRequestMessage = new(HttpMethod.Get, queryStringBuilder.ToString());
				HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(getPeopleAddressRequestMessage, cancellationToken);

				if (!httpResponseMessage.IsSuccessStatusCode)
				{
					exceptionCount++;
					continue;
				}

				PeopleAddressHttpResponse? peopleAddressHttpResponse = await httpResponseMessage.Content.ReadFromJsonAsync<PeopleAddressHttpResponse>(new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

				if (peopleAddressHttpResponse == null || peopleAddressHttpResponse.Data == null || peopleAddressHttpResponse.Data.Count == 0)
				{
					exceptionCount++;
					continue;
				}

				List<ProcessedTehranCityAddress> processedTehranCityAddresses = new();

				var personAddressIds = peopleAddressHttpResponse.Data.Select(x => (int?)x.Id).ToList();

				var existPersonAddressIds = await _processDataDbContext!.Set<ProcessedTehranCityAddress>()
					.Select(x => x.AddressId)
					.Where(x => personAddressIds.Contains(x)).ToListAsync(cancellationToken);

				var personAddressNewData = peopleAddressHttpResponse.Data.Where(x => !existPersonAddressIds.Contains(x.Id)).ToList();

				if (personAddressNewData.Count > 0)
				{
					foreach (var item in personAddressNewData)
					{
						processedTehranCityAddresses.Add(new ProcessedTehranCityAddress()
						{
							AddressId = item.Id,
							Address = item.Detail,
							CityName = item.City?.Name,
							ProvinceName = item.Province?.Name
						});
					}

					await _processDataDbContext!.AddRangeAsync(processedTehranCityAddresses, cancellationToken);
					await _processDataDbContext.SaveChangesAsync(cancellationToken);
				}

				page++;
				exceptionCount = 0;
				hasData = page * pageSize < peopleAddressHttpResponse.TotalCount;
			}

			Console.WriteLine("reading from web finished");
		}

		protected override void OnDestroying() { }
	}
}

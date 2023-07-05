using iHome.Infrastructure.Queue.Models;
using iHome.Infrastructure.Queue.Service.Read;
using iHome.Infrastructure.Queue.Service.Write;
using iHome.Jobs.Events.EventsExecutor.Services;
using iHome.Microservices.Devices.Contract;
using Microsoft.ApplicationInsights;

namespace iHome.Jobs.Events.EventsExecutor
{
    public class Worker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        private readonly TelemetryClient _telemetryClient;
        private readonly PeriodicTimer _timer;
        private readonly TimeSpan Delay = TimeSpan.FromSeconds(5);
        private const int BatchSize = 1000;

        private IScheduleDevicesProvider _provider = default!;
        private IScheduleDevicesProcessor _processor = default!;

        public Worker(IHostApplicationLifetime hostApplicationLifetime, IServiceScopeFactory serviceScopeFactory, TelemetryClient telemetryClient)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _serviceScopeFactory = serviceScopeFactory;
            _telemetryClient = telemetryClient;

            _timer = new PeriodicTimer(Delay);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                InitializeScope();
                while (await _timer.WaitForNextTickAsync(stoppingToken))
                {
                    var devices = await _provider.Provide();
                    
                    var tasks = devices.Chunk(BatchSize)
                        .Select(_processor.Process)
                        .ToArray();

                    await Task.WhenAll(tasks);

                    var eventsCount = tasks.Select(t => t.Result).Sum();

                    if (eventsCount == 0) continue;
                    _telemetryClient.TrackEvent(typeof(Worker).FullName, new Dictionary<string, string> { { "Events executed", eventsCount.ToString() } });
                }
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
            }
            finally
            {
                _hostApplicationLifetime.StopApplication();
            }
        }

        public void InitializeScope()
        {
            var scope = _serviceScopeFactory.CreateScope();
            _provider = scope.ServiceProvider.GetRequiredService<IScheduleDevicesProvider>();
            _processor = scope.ServiceProvider.GetRequiredService<IScheduleDevicesProcessor>();

            if (_provider is null || _processor is null)
            {
                throw new ArgumentNullException();
            }
        }
    }
}
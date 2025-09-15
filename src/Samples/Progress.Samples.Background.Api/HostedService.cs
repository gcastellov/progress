using Progress.Builders;
using Progress.Reporters;

namespace Progress.Samples.Background.Api;

internal class HostedService : IHostedService
{
    private readonly Worker _worker = new();
    private readonly BackgroundReporter _reporter;

    public HostedService(ILogger<HostedService> logger)
    {
        _reporter = new BackgroundReporterBuilder()
            .NotifyingProgress((stats) =>
            {
                logger.LogDebug("Getting stats on progress {percent}", stats.CurrentPercent);
            })
            .NotifyingCompletion((stats) =>
            {
                logger.LogDebug("Getting stats on completion");
            })
            .Build(Worker.AllItems);

        _worker.OnSuccess = () => _reporter.ReportSuccess();
        _worker.OnFailure = () => _reporter.ReportFailure();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {        
        _reporter.Start();
        await _worker.DoMywork();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _reporter.Stop();
        return Task.CompletedTask;
    }
}

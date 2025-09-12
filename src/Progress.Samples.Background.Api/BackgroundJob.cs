
using Progress.Builders;

namespace Progress.Samples.Background.Api;

internal class BackgroundJob(ILogger<BackgroundJob> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var onProgress = (Stats stats) =>
        {
            // TODO: Do something useful
            logger.LogDebug("Getting stats on progress {percent}", stats.CurrentPercent);
        };

        var onCompletion = (Stats stats) =>
        {
            // TODO: Do something useful
            logger.LogDebug("Getting stats on completion");
        };

        var reporter = new BackgroundReporterBuilder()
            .NotifyingProgress(onProgress, TimeSpan.FromSeconds(10))
            .NotifyingCompletion(onCompletion)
            .ExportingTo("output.json", Settings.FileType.Json)
            .Build(Worker.AllItems);


        var worker = new Worker()
        {
            OnSuccess = () => reporter.ReportSuccess(),
            OnFailure = () => reporter.ReportFailure(),
        };

        reporter.Start();
        await worker.DoMywork();
    }
}

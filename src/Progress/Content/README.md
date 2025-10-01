![Progress](logo512x512.png)

# Progress

This library provides a set of features that help you spin up reporting tasks, either in the form of console apps that show workloads progression or via background jobs providing access to stats during the lifespan of the workload.

## Key components
- Reporters are the core components that display information about the current workload status.
    - **ConsoleReporter**: Used in console apps to report progression of a simple workload.
    - **ConsoleAggregateReporter**: Used in console apps to report progression of many workloads.
    - **BackgroundReporter**: Used in background services that don't require to ouput to std out.

- Components are objects that report progress as figures, such as bars, spinners, pulse ... Only supported for console-based reporters. All these components are internal and they can only be initialized via their Descriptors.
    - Bar
    - Spinner
    - HearthBeat
    - Pulse

- Descriptors are builders that help to set up components with proper appearance and behavior.
    - BarDescriptor
    - SpinnerDescriptor
    - HearthBeatDescriptor
    - PulseDescriptor

- ReporterBuilders help to set up reporters with workloads, components and behavior.
    - ConsoleReporterBuilder
    - ConsoleAggregateReporterBuilder
    - BackgroundReporterBuilder

## Console apps with ConsoleReporter

```csharp
        using var reporter = new ConsoleReporterBuilder()
            .DisplayingStartingTime()
            .DisplayingElapsedTime()
            .DisplayingTimeOfArrival()
            .DisplayingRemainingTime()
            .DisplayingItemsSummary()
            .DisplayingItemsOverview()
            .NotifyingProgress(OnProgress)
            .NotifyingCompletion(OnCompletion)
            .ExportingTo("output.json", FileType.Json)
            .UsingReportingFrequency(TimeSpan.FromMilliseconds(50))
            .UsingComponentDescriptor(BarDescriptor.Default)
            .UsingExpectedItems(SimpleWorker.ExpectedItems)
            .Build();

        var worker = new SimpleWorker()
        {
            OnSuccess = reporter.ReportSuccess,
            OnFailure = reporter.ReportFailure,
        };

        reporter.Start();
        await worker.DoMyworkAsync();
```

## Console apps with ConsoleAggregateReporter

```csharp
    var worker = new InstallerWorker();

    using var reporter = new ConsoleAggregateReporterBuilder()
        .DisplayingStartingTime()
        .DisplayingElapsedTime()
        .DisplayingTimeOfArrival()
        .DisplayingRemainingTime()
        .DisplayingItemsSummary()
        .DisplayingItemsOverview()
        .NotifyingProgress(OnProgress)
        .NotifyingCompletion(OnCompletion)
        .ExportingTo("output.json", FileType.Json)
        .UsingReportingFrequency(TimeSpan.FromMilliseconds(50))
        .UsingWorkload(worker.CalcRequirements, BarDescriptor.Default)
        .UsingWorkload(worker.DownloadArtifacts, BarDescriptor.Default)
        .UsingWorkload(worker.InstallArtifacts, BarDescriptor.Default)
        .Build();

    worker.OnSuccess = reporter.ReportSuccess;
    worker.OnFailure = reporter.ReportFailure;

    reporter.Start();
    await worker.CalcRequirements.CalcAsync();
    Task[] restTasks = [worker.DownloadArtifacts.DownloadAsync(), worker.InstallArtifacts.InstallAsync()];
    await Task.WhenAll(restTasks);
```


## Background services

```csharp
internal class HostedService : IHostedService
{
    private readonly SimpleWorker _worker = new();
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
            .Build(SimpleWorker.ExpectedItems);

        _worker.OnSuccess = () => _reporter.ReportSuccess();
        _worker.OnFailure = () => _reporter.ReportFailure();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {        
        _reporter.Start();
        await _worker.DoMyworkAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _reporter.Stop();
        return Task.CompletedTask;
    }
}
```
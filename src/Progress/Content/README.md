# Progress

This library helps you to spin up reporters for giving an overview of the progression of a given operation. These reporters can either be for console apps that output the information or background jobs hosted by APIs handling the progress via hooks. 


## Report progress for console apps
Report progression with ease by using multilple components such as Bars, Spinners, Pulse and more.

```csharp
using var reporter = new ConsoleReporterBuilder()
    .UsingReportingFrequency(TimeSpan.FromMilliseconds(50))
    .UsingComponentDescriptor(BarDescriptor.Default)
    .Build(Worker.AllItems);

var worker = new Worker()
{
    OnSuccess = () => reporter.ReportSuccess(),
    OnFailure = () => reporter.ReportFailure(),
};

reporter.Start();
await worker.DoMywork();
```

### Define the component to show progress
Components are subjected to the ConsoleReporter.

```csharp
var descriptor = new BarDescriptor()
    .UsingProgressSymbol('#')
    .UsingWidth(50)
    .DisplayingPercent();
```

or use the default one
```csharp
var descriptor = BarDescriptor.Default;
```

### Define the reporter to use the component
```csharp
var reporter = new ConsoleReporterBuilder()
    .DisplayingStartingTime()
    .DisplayingElapsedTime()
    .DisplayingTimeOfArrival()
    .DisplayingRemainingTime()
    .DisplayingItemsSummary()
    .DisplayingItemsOverview()
    .NotifyingProgress(onProgress)
    .NotifyingCompletion(onCompletion)
    .ExportingTo("output.json", FileType.Json)
    .UsingReportingFrequency(TimeSpan.FromMilliseconds(50))
    .UsingComponentDescriptor(BarDescriptor.Default)
    .Build(Worker.AllItems);
```

## Report progress for background jobs
In case your workload happens in the background and you only pretend to collect progression status during certain moments of the execution, or export the results at the end, the BackgroundReporter may help you to remove all the boilerplate.

```csharp
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
```

### Start the reporter
```csharp
reporter.Start();
```

### Stop the reporter
```csharp
reporter.Stop();
```

### Resume the reporter
```csharp
reporter.Resume();
```

### Report progress
```csharp
reporter.ReportSuccess()
reporter.ReportFailure()
```

### Collect stats while running
```csharp
var onProgress = (Stats stats) =>
{
    // TODO: Do something useful
};

var onCompletion = (Stats stats) =>
{
    // TODO: Do something useful
};

var reporter = new ConsoleReporterBuilder()
    .NotifyingProgress(onProgress)
    .NotifyingCompletion(onCompletion)
    .Build(Worker.AllItems);
```


### Exports final stats (json, csv, xml, txt)
```csharp
var reporter = new ConsoleReporterBuilder()
    .ExportingTo("output.json", FileType.Json)
    .Build(Worker.AllItems);
```
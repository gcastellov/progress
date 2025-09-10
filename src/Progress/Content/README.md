# Progress

Report progression with ease by using multilple components such as Bars, Spinners, Pulse and more


## Report progress for console apps

```csharp
using var reporter = new ReporterBuilder()
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
var reporter = new ReporterBuilder()
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

var reporter = new ReporterBuilder()
    .NotifyingProgress(onProgress)
    .NotifyingCompletion(onCompletion)
    .Build(Worker.AllItems);
```


### Exports final stats (json, csv, xml, txt)
```csharp
var reporter = new ReporterBuilder()
    .ExportingTo("output.json", FileType.Json)
    .Build(Worker.AllItems);
```

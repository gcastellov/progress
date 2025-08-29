# Progress

Report progression with ease by using multilple components such as Bars, Spinners, Pulse and more

![Progress](img/logo512x512.png)

## Report progress for console apps

```csharp
using var reporter = new ReporterBuilder()
    .DisplayingStartingTime()
    .DisplayingElapsedTime()
    .DisplayingTimeOfArrival()
    .DisplayingRemainingTime()
    .DisplayingItemsSummary()
    .DisplayingItemsOverview()
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
    .UsingReportingFrequency(TimeSpan.FromMilliseconds(50))
    .UsingComponentDescriptor(descriptor)
    .Build(Worker.AllItems);
```

### Start the reporter
```csharp
reporter.Start();
```

### Report progress
```csharp
reporter.ReportSuccess()
reporter.ReportFailure()
```

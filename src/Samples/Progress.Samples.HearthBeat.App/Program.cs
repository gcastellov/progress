using Progress;
using Progress.Builders;
using Progress.Descriptors;
using Progress.Samples;
using Progress.Settings;

var onProgress = (Stats stats) =>
{
    // TODO: Do something useful
};

var onCompletion = (Stats stats) =>
{
    // TODO: Do something useful
};

using var reporter = new ConsoleReporterBuilder()
    .DisplayingStartingTime()
    .DisplayingElapsedTime()
    .DisplayingTimeOfArrival()
    .DisplayingRemainingTime()
    .DisplayingItemsSummary()
    .DisplayingItemsOverview()
    .NotifyingProgress(onProgress)
    .NotifyingCompletion(onCompletion)    
    .ExportingTo("output.csv", FileType.Csv)
    .UsingReportingFrequency(TimeSpan.FromMilliseconds(50))
    .UsingComponentDescriptor(HearthBeatDescriptor.Default)
    .UsingExpectedItems(SimpleWorker.ExpectedItems)
    .Build();

var worker = new SimpleWorker()
{
    OnSuccess = () => reporter.ReportSuccess(),
    OnFailure = () => reporter.ReportFailure(),
};

reporter.Start();
await worker.DoMyworkAsync();
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
    .ExportingTo("output.txt", FileType.Text)
    .UsingReportingFrequency(TimeSpan.FromMilliseconds(50))
    .UsingComponentDescriptor(PulseDescriptor.Default)
    .UsingExpectedItems(SimpleWorker.ExpectedItems)
    .Build();

var worker = new SimpleWorker()
{
    OnSuccess = () => reporter.ReportSuccess(),
    OnFailure = () => reporter.ReportFailure(),
};

reporter.Start();
await worker.DoMyworkAsync();
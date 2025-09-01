using Progress;
using Progress.Descriptors;
using Progress.Samples;

var onStatsNotified = (Stats stats) =>
{
    // TODO: Implement collect
};

using var reporter = new ReporterBuilder()
    .DisplayingStartingTime()
    .DisplayingElapsedTime()
    .DisplayingTimeOfArrival()
    .DisplayingRemainingTime()
    .DisplayingItemsSummary()
    .DisplayingItemsOverview()
    .NotifyingStats(onStatsNotified)
    .UsingReportingFrequency(TimeSpan.FromMilliseconds(50))
    .UsingComponentDescriptor(SpinnerDescriptor.Default)
    .Build(Worker.AllItems);

var worker = new Worker()
{
    OnSuccess = () => reporter.ReportSuccess(),
    OnFailure = () => reporter.ReportFailure(),
};

reporter.Start();
await worker.DoMywork();
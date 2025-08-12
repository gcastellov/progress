using Progress;
using Progress.Descriptors;
using Progress.Samples;

using var reporter = new ReporterBuilder()
    .DisplayingStartingTime()
    .DisplayingElapsedTime()
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
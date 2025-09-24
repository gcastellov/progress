using Progress.Builders;
using Progress.Descriptors;
using Progress.Reporters;
using Progress.Settings;

namespace Progress.Samples.Bar.App;

internal static class ReporterFactory
{
    private static Action<Stats> OnProgress = (Stats stats) =>
    {
        // TODO: Do something useful
    };

    private static Action<Stats> OnCompletion = (Stats stats) =>
    {
        // TODO: Do something useful
    };

    public static ConsoleReporter GetConsoleReporterBuilder()
    {
        return new ConsoleReporterBuilder()
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
    }

    public static ConsoleAggregateReporter GetConsoleAggregateReporterBuilder(InstallerWorker worker)
    {
        return new ConsoleAggregateReporterBuilder()
            .DisplayingStartingTime()
            .DisplayingElapsedTime()
            .DisplayingTimeOfArrival()
            .DisplayingRemainingTime()
            .NotifyingProgress(OnProgress)
            .NotifyingCompletion(OnCompletion)
            .ExportingTo("output.json", FileType.Json)
            .UsingReportingFrequency(TimeSpan.FromMilliseconds(50))
            .UsingWorkload(worker.CalcRequirements, BarDescriptor.Default)
            .UsingWorkload(worker.DownloadArtifacts, BarDescriptor.Default)
            .UsingWorkload(worker.InstallArtifacts, BarDescriptor.Default)
            .Build();
    }
}

using Progress.Builders;
using Progress.Descriptors;
using Progress.Reporters;
using Progress.Settings;
using static Progress.Samples.InstallerWorker;

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
            .Build(SimpleWorker.ExpectedItems);
    }


    public static ConsoleAggregateReporter GetConsoleAggregateReporterBuilder()
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
            .UsingWorkload(CalcRequirements.Name, CalcRequirements.Description, CalcRequirements.ExpectedItems, BarDescriptor.Default)
            .UsingWorkload(DonwloadArtifacts.Name, DonwloadArtifacts.Description, DonwloadArtifacts.ExpectedItems, BarDescriptor.Default)
            .UsingWorkload(InstallArtifacts.Name, InstallArtifacts.Description, InstallArtifacts.ExpectedItems, BarDescriptor.Default)
            .Build();
    }
}

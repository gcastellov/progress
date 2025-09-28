using Progress;
using Progress.Builders;
using Progress.Descriptors;
using Progress.Samples;
using Progress.Samples.Utils;
using Progress.Settings;

class Program
{
    private static Action<Stats> OnProgress = (Stats stats) =>
    {
        // TODO: Do something useful
    };

    private static Action<Stats> OnCompletion = (Stats stats) =>
    {
        // TODO: Do something useful
    };

    static async Task Main(string[] args)
    {
        var task = ConsoleUtils.UseAggregateReporter(args) switch
        {
            true => RunInstallerSample(),
            false => RunSimpleSample(),
        };

        await task;
    }

    private async static Task RunSimpleSample()
    {
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
            .UsingComponentDescriptor(SpinnerDescriptor.Default)
            .UsingExpectedItems(SimpleWorker.ExpectedItems)
            .Build();

        var worker = new SimpleWorker()
        {
            OnSuccess = reporter.ReportSuccess,
            OnFailure = reporter.ReportFailure,
        };

        reporter.Start();
        await worker.DoMyworkAsync();
    }

    private async static Task RunInstallerSample()
    {
        var worker = new InstallerWorker();

        using var reporter = new ConsoleAggregateReporterBuilder()
            .DisplayingStartingTime()
            .DisplayingElapsedTime()
            .DisplayingTimeOfArrival()
            .DisplayingRemainingTime()
            .NotifyingProgress(OnProgress)
            .NotifyingCompletion(OnCompletion)
            .ExportingTo("output.json", FileType.Json)
            .UsingReportingFrequency(TimeSpan.FromMilliseconds(50))
            .UsingWorkload(worker.CalcRequirements, SpinnerDescriptor.Default)
            .UsingWorkload(worker.DownloadArtifacts, SpinnerDescriptor.Default)
            .UsingWorkload(worker.InstallArtifacts, SpinnerDescriptor.Default)
            .Build();

        worker.OnSuccess = reporter.ReportSuccess;
        worker.OnFailure = reporter.ReportFailure;

        reporter.Start();
        await worker.CalcRequirements.CalcAsync();
        Task[] restTasks = [worker.DownloadArtifacts.DownloadAsync(), worker.InstallArtifacts.InstallAsync()];
        await Task.WhenAll(restTasks);
    }
}
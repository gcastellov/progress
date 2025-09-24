using Progress.Descriptors;
using Progress.Reporters;

namespace Progress.Builders;

/// <summary>
/// Builder for helping to set up the console reporter with the desired behavior.
/// It will create an instance of <see cref="ConsoleReporter"/> using a <see cref="BarDescriptor"/> by default.
/// </summary>
public class ConsoleReporterBuilder : ConsoleReporterBuilderBase<ConsoleReporter,  ConsoleReporterBuilder>
{
    private ComponentDescriptor _componentDescriptor = BarDescriptor.Default;
    private ulong _expectedItemsCount;

    /// <summary>
    /// Sets the expected items count to process
    /// </summary>
    /// <param name="expectedItemsCount"></param>
    /// <returns></returns>
    public ConsoleReporterBuilder UsingExpectedItems(ulong expectedItemsCount)
    {
        _expectedItemsCount = expectedItemsCount;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="ComponentDescriptor"/> being used to render the progress of the operation.
    /// </summary>
    /// <param name="descriptor"></param>
    /// <returns></returns>
    public ConsoleReporterBuilder UsingComponentDescriptor(ComponentDescriptor descriptor)
    {
        _componentDescriptor = descriptor;
        return this;
    }
        

    /// <summary>
    /// Builds the reporter getting an instance of <see cref="ConsoleReporter"/>.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public override ConsoleReporter Build()
    {
        if (_expectedItemsCount == 0)
            throw new ArgumentException("Nothing to do! Set the expected items count for completion.");

        var component = _componentDescriptor.Build();

        var workload = Workload.Default(_expectedItemsCount);
        workload.Component = component;

        var reporter = new ConsoleReporter(workload)
        {
            OnProgress = _onProgressNotified,
            OnCompletion = _onCompletionNotified
        };

        reporter.Configuration.ReportFrequency = _reportFrequency;
        reporter.Configuration.StatsFrequency = _statsFrequency;
        reporter.Configuration.ExportSettings = _exportSettings;
        reporter.Configuration.Options.DisplayRemainingTime = _displayRemainingTime;
        reporter.Configuration.Options.DisplayEstimatedTimeOfArrival = _displayEstTimeOfArrival;
        reporter.Configuration.Options.DisplayElapsedTime = _displayElapsedTime;
        reporter.Configuration.Options.DisplayStartingTime = _displayStartingTime;
        reporter.Configuration.Options.DisplayItemsOverview = _displayItemsOverview;
        reporter.Configuration.Options.DisplayItemsSummary = _displayItemsSummary;
        reporter.Configuration.Options.NotifyProgressStats = _onProgressNotified != null;
        reporter.Configuration.Options.NotifyCompletionStats = _onCompletionNotified != null;
        reporter.Configuration.Options.ExportCompletionStats = _exportSettings != null;

        return reporter;
    }
}

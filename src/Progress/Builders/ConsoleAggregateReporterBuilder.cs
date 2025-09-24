using Progress.Descriptors;
using Progress.Reporters;

namespace Progress.Builders;

/// <summary>
/// Builder for helping to set up the console aggregate reporter with the desired behavior.
/// It will create an instance of <see cref="ConsoleAggregateReporter"/> using a <see cref="BarDescriptor"/> by default.
/// </summary>
public class ConsoleAggregateReporterBuilder : ConsoleReporterBuilderBase<ConsoleAggregateReporter, ConsoleAggregateReporterBuilder>
{
    private Dictionary<string, Workload> _workloads = new();


    /// <summary>
    /// Sets the workload with its name, description, expected items and the <see cref="ComponentDescriptor"/> being used to render the progress of the operation.
    /// </summary>
    /// <param name="workload"></param>
    /// <param name="descriptor"></param>
    /// <returns></returns>
    public ConsoleAggregateReporterBuilder UsingWorkload(Workload workload, ComponentDescriptor descriptor)
    {
        if (workload.ItemsCount == 0)
            throw new ArgumentException("Nothing to do! Set the expected items count for completion.");

        if (_workloads.ContainsKey(workload.Id))
            throw new InvalidOperationException($"A workload with id {workload.Id} is already set up. Please, use a different one.");

        workload.Component = descriptor.Build();
        _workloads.Add(workload.Id, workload);
        return this;
    }

    /// <summary>
    /// Builds the reporter getting an instance of <see cref="ConsoleAggregateReporter"/>.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public override ConsoleAggregateReporter Build()
    {
        var reporter = new ConsoleAggregateReporter(_workloads.Values)
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
        reporter.Configuration.Options.HideWorkflowOnComplete = _hideOnComplete;
        reporter.Configuration.Options.NotifyProgressStats = _onProgressNotified != null;
        reporter.Configuration.Options.NotifyCompletionStats = _onCompletionNotified != null;
        reporter.Configuration.Options.ExportCompletionStats = _exportSettings != null;

        return reporter;
    }
}

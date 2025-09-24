using Progress.Descriptors;
using Progress.Reporters;
using Progress.Settings;

namespace Progress.Builders;

/// <summary>
/// Builder for helping to set up the console aggregate reporter with the desired behavior.
/// It will create an instance of <see cref="ConsoleAggregateReporter"/> using a <see cref="BarDescriptor"/> by default.
/// </summary>
public class ConsoleAggregateReporterBuilder
{
    private bool _displayRemainingTime;
    private bool _displayEstTimeOfArrival;
    private bool _displayElapsedTime;
    private bool _displayStartingTime;
    private bool _displayItemsOverview;
    private bool _displayItemsSummary;
    private bool _hideOnComplete;
    private TimeSpan _reportFrequency = TimeSpan.FromSeconds(1);
    private TimeSpan _statsFrequency = TimeSpan.FromSeconds(5);
    private Dictionary<string, Workload> _workloads = new();
    private ExportSettings _exportSettings = default!;
    private Action<Stats> _onProgressNotified = default!;
    private Action<Stats> _onCompletionNotified = default!;

    /// <summary>
    /// The reporter will display the elapsed time since the start.
    /// </summary>
    /// <returns></returns>
    public ConsoleAggregateReporterBuilder DisplayingElapsedTime()
    {
        _displayElapsedTime = true;
        return this;
    }

    /// <summary>
    /// The reporter will display the remaining time to finish.
    /// </summary>
    /// <returns></returns>
    public ConsoleAggregateReporterBuilder DisplayingRemainingTime()
    {
        _displayRemainingTime = true;
        return this;
    }

    /// <summary>
    /// The reporter will display when the operation is expected to finish.
    /// </summary>
    /// <returns></returns>
    public ConsoleAggregateReporterBuilder DisplayingTimeOfArrival()
    {
        _displayEstTimeOfArrival = true;
        return this;
    }

    /// <summary>
    /// The reporter will display when the operation started.
    /// </summary>
    /// <returns></returns>
    public ConsoleAggregateReporterBuilder DisplayingStartingTime()
    {
        _displayStartingTime = true;
        return this;
    }

    /// <summary>
    /// The rerporter will display the total of items processed.
    /// </summary>
    /// <returns></returns>
    public ConsoleAggregateReporterBuilder DisplayingItemsOverview()
    {
        _displayItemsOverview = true;
        return this;
    }

    /// <summary>
    /// The reporter will display the amount of success and failures.
    /// </summary>
    /// <returns></returns>
    public ConsoleAggregateReporterBuilder DisplayingItemsSummary()
    {
        _displayItemsSummary = true;
        return this;
    }

    /// <summary>
    /// The reporter will hide the workload progression when it gets the 100%.
    /// </summary>
    /// <param name="hide"></param>
    /// <returns></returns>
    public ConsoleAggregateReporterBuilder HideWorkloadOnComplete(bool hide)
    {
        _hideOnComplete = hide;
        return this;
    }

    /// <summary>
    /// Sets the frequency the reporter will refresh the status of the operation.
    /// The default reporting frequency is set to 1s.
    /// </summary>
    /// <param name="frequency"></param>
    /// <returns></returns>
    public ConsoleAggregateReporterBuilder UsingReportingFrequency(TimeSpan frequency)
    {
        _reportFrequency = frequency;
        return this;
    }

    /// <summary>
    /// Sets the workload with its name, description, expected items and the <see cref="ComponentDescriptor"/> being used to render the progress of the operation.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="expectedItems"></param>
    /// <param name="descriptor"></param>
    /// <returns></returns>
    public ConsoleAggregateReporterBuilder UsingWorkload(string name, string description, ulong expectedItems, ComponentDescriptor descriptor)
    {
        if (expectedItems == 0)
            throw new ArgumentException("Nothing to do! Set the expected items count for completion.");

        if (_workloads.ContainsKey(name))
            throw new InvalidOperationException($"A workload with name {name} is already set up. Please, use a different one.");

        _workloads.Add(name, new Workload(name, description, expectedItems, descriptor.Build()));
        return this;
    }

    /// <summary>
    /// Sets the progress notification callback.
    /// Default notification frequency is set to 5 seconds.
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public ConsoleAggregateReporterBuilder NotifyingProgress(Action<Stats> callback)
    {
        return NotifyingProgress(callback, _statsFrequency);
    }

    /// <summary>
    /// Sets the progress notification callback with the invocation frequency.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="statsFrequency"></param>
    /// <returns></returns>
    public ConsoleAggregateReporterBuilder NotifyingProgress(Action<Stats> callback, TimeSpan statsFrequency)
    {
        _onProgressNotified = callback;
        _statsFrequency = statsFrequency;
        return this;
    }

    /// <summary>
    /// Sets the completion notification callback.
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public ConsoleAggregateReporterBuilder NotifyingCompletion(Action<Stats> callback)
    {
        _onCompletionNotified = callback;
        return this;
    }

    /// <summary>
    /// Sets and tells the reporter how and where to export the final stats.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="fileType"></param>
    /// <returns></returns>
    public ConsoleAggregateReporterBuilder ExportingTo(string fileName, FileType fileType)
    {
        _exportSettings = new ExportSettings(fileName, fileType);
        return this;
    }


    /// <summary>
    /// Builds the reporter getting an instance of <see cref="ConsoleAggregateReporter"/>.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public ConsoleAggregateReporter Build()
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

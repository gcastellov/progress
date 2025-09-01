using Progress.Descriptors;

namespace Progress;

/// <summary>
/// Builder for helping to set up the reported with the desired behavior.
/// It will create an instance of <see cref="Reporter"/> using a <see cref="BarDescriptor"/> by default.
/// </summary>
public class ReporterBuilder
{
    private bool _displayRemainingTime;
    private bool _displayEstTimeOfArrival;
    private bool _displayElapsedTime;
    private bool _displayStartingTime;
    private bool _displayItemsOverview;
    private bool _displayItemsSummary;
    private bool _notifyingStats;
    private TimeSpan _reportFrequency = TimeSpan.FromSeconds(1);
    private TimeSpan _statsFrequency = TimeSpan.FromSeconds(5);
    private ComponentDescriptor _componentDescriptor = BarDescriptor.Default;
    private Action<Stats> _onStatsNotified = default!;

    /// <summary>
    /// The reporter will display the elapsed time since the start.
    /// </summary>
    /// <returns></returns>
    public ReporterBuilder DisplayingElapsedTime()
    {
        _displayElapsedTime = true;
        return this;
    }

    /// <summary>
    /// The reporter will display the remaining time to finish.
    /// </summary>
    /// <returns></returns>
    public ReporterBuilder DisplayingRemainingTime()
    {
        _displayRemainingTime = true;
        return this;
    }

    /// <summary>
    /// The reporter will display when the operation is expected to finish.
    /// </summary>
    /// <returns></returns>
    public ReporterBuilder DisplayingTimeOfArrival()
    {
        _displayEstTimeOfArrival = true;
        return this;
    }

    /// <summary>
    /// The reporter will display when the operation started.
    /// </summary>
    /// <returns></returns>
    public ReporterBuilder DisplayingStartingTime()
    {
        _displayStartingTime = true;
        return this;
    }

    /// <summary>
    /// The rerporter will display the total of items processed.
    /// </summary>
    /// <returns></returns>
    public ReporterBuilder DisplayingItemsOverview()
    {
        _displayItemsOverview = true;
        return this;
    }

    /// <summary>
    /// The reporter will display the amount of success and failures.
    /// </summary>
    /// <returns></returns>
    public ReporterBuilder DisplayingItemsSummary()
    {
        _displayItemsSummary = true;
        return this;
    }

    /// <summary>
    /// Sets the frequency the reporter will refresh the status of the operation.
    /// The default reporting frequency is set to 1s.
    /// </summary>
    /// <param name="frequency"></param>
    /// <returns></returns>
    public ReporterBuilder UsingReportingFrequency(TimeSpan frequency)
    {
        _reportFrequency = frequency;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="ComponentDescriptor"/> being used to render the progress of the operation.
    /// </summary>
    /// <param name="descriptor"></param>
    /// <returns></returns>
    public ReporterBuilder UsingComponentDescriptor(ComponentDescriptor descriptor)
    {
        _componentDescriptor = descriptor;
        return this;
    }

    /// <summary>
    /// Sets the stats notification callback.
    /// Default notification frequency is set to 5 seconds.
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public ReporterBuilder NotifyingStats(Action<Stats> callback)
    {
        return NotifyingStats(callback, _statsFrequency);
    }

    /// <summary>
    /// Sets the stats notification callback with the invocation frequency .
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="statsFrequency"></param>
    /// <returns></returns>
    public ReporterBuilder NotifyingStats(Action<Stats> callback, TimeSpan statsFrequency)
    {
        _onStatsNotified = callback;
        _statsFrequency = statsFrequency;
        _notifyingStats = true;
        return this;
    }

    /// <summary>
    /// Builds the reporte getting an instance of <see cref="Reporter"/>.
    /// </summary>
    /// <param name="itemsCount"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public Reporter Build(ulong itemsCount)
    {
        if (itemsCount == 0)
            throw new ArgumentException("Nothing to do! Set the initial items count for completion.");

        var component = _componentDescriptor.Build();
        
        return new Reporter(itemsCount, component)
        {
            DisplayRemainingTime = _displayRemainingTime,
            DisplayEstimatedTimeOfArrival = _displayEstTimeOfArrival,
            DisplayElapsedTime = _displayElapsedTime,
            DisplayStartingTime = _displayStartingTime,
            DisplayItemsOverview = _displayItemsOverview,
            DisplayItemsSummary = _displayItemsSummary,
            NotifyStats = _notifyingStats,
            ReportFrequency = _reportFrequency,
            StatsFrequency = _statsFrequency,
            OnStatsNotified = _onStatsNotified,
        };
    }
}

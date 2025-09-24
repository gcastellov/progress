using Progress.Reporters;
using Progress.Settings;

namespace Progress.Builders;

/// <summary>
/// Base class for console based reporter builders
/// </summary>
public abstract class ConsoleReporterBuilderBase<T, U>  
    where T: ConsoleReporterBase
    where U: ConsoleReporterBuilderBase<T, U>
{
    /// <summary>
    /// Indicates whether it displays the remaining time
    /// </summary>
    protected bool _displayRemainingTime;

    /// <summary>
    /// Indicates whether it displays the ETA
    /// </summary>
    protected bool _displayEstTimeOfArrival;

    /// <summary>
    /// Indicates whether it displays the elapsed time
    /// </summary>
    protected bool _displayElapsedTime;

    /// <summary>
    /// Indicates whether it displays the starting time
    /// </summary>
    protected bool _displayStartingTime;

    /// <summary>
    /// Indicates whether it displays the items overview
    /// </summary>
    protected bool _displayItemsOverview;

    /// <summary>
    /// Indicates whether it displays the items summary
    /// </summary>
    protected bool _displayItemsSummary;

    /// <summary>
    /// Indicates whether it shows or hides the progression component once it has finished 
    /// </summary>
    protected bool _hideOnComplete;

    /// <summary>
    /// The reporting frequency timespan
    /// </summary>
    protected TimeSpan _reportFrequency = TimeSpan.FromSeconds(1);
    
    /// <summary>
    /// The stats frequency timespan
    /// </summary>
    protected TimeSpan _statsFrequency = TimeSpan.FromSeconds(5);
    
    /// <summary>
    /// The export settings
    /// </summary>
    protected ExportSettings _exportSettings = default!;
    
    /// <summary>
    /// The stats hook called during the progression
    /// </summary>
    protected Action<Stats> _onProgressNotified = default!;

    /// <summary>
    /// The stats hook called on comppletion
    /// </summary>
    protected Action<Stats> _onCompletionNotified = default!;

    /// <summary>
    /// The reporter will display the elapsed time since the start.
    /// </summary>
    /// <returns></returns>
    public U DisplayingElapsedTime()
    {
        _displayElapsedTime = true;
        return (U)this;
    }

    /// <summary>
    /// The reporter will display the remaining time to finish.
    /// </summary>
    /// <returns></returns>
    public U DisplayingRemainingTime()
    {
        _displayRemainingTime = true;
        return (U)this;
    }

    /// <summary>
    /// The reporter will display when the operation is expected to finish.
    /// </summary>
    /// <returns></returns>
    public U DisplayingTimeOfArrival()
    {
        _displayEstTimeOfArrival = true;
        return (U)this;
    }

    /// <summary>
    /// The reporter will display when the operation started.
    /// </summary>
    /// <returns></returns>
    public U DisplayingStartingTime()
    {
        _displayStartingTime = true;
        return (U)this;
    }

    /// <summary>
    /// The rerporter will display the total of items processed.
    /// </summary>
    /// <returns></returns>
    public U DisplayingItemsOverview()
    {
        _displayItemsOverview = true;
        return (U)this;
    }

    /// <summary>
    /// The reporter will display the amount of success and failures.
    /// </summary>
    /// <returns></returns>
    public U DisplayingItemsSummary()
    {
        _displayItemsSummary = true;
        return (U)this;
    }

    /// <summary>
    /// The reporter will hide the workload progression when it gets the 100%.
    /// </summary>
    /// <param name="hide"></param>
    /// <returns></returns>
    public U HideWorkloadOnComplete(bool hide)
    {
        _hideOnComplete = hide;
        return (U)this;
    }

    /// <summary>
    /// Sets the frequency the reporter will refresh the status of the operation.
    /// The default reporting frequency is set to 1s.
    /// </summary>
    /// <param name="frequency"></param>
    /// <returns></returns>
    public U UsingReportingFrequency(TimeSpan frequency)
    {
        _reportFrequency = frequency;
        return (U)this;
    }

    /// <summary>
    /// Sets the progress notification callback.
    /// Default notification frequency is set to 5 seconds.
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public U NotifyingProgress(Action<Stats> callback)
    {
        return NotifyingProgress(callback, _statsFrequency);
    }

    /// <summary>
    /// Sets the progress notification callback with the invocation frequency.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="statsFrequency"></param>
    /// <returns></returns>
    public U NotifyingProgress(Action<Stats> callback, TimeSpan statsFrequency)
    {
        _onProgressNotified = callback;
        _statsFrequency = statsFrequency;
        return (U)this;
    }

    /// <summary>
    /// Sets the completion notification callback.
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public U NotifyingCompletion(Action<Stats> callback)
    {
        _onCompletionNotified = callback;
        return (U)this;
    }

    /// <summary>
    /// Sets and tells the reporter how and where to export the final stats.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="fileType"></param>
    /// <returns></returns>
    public U ExportingTo(string fileName, FileType fileType)
    {
        _exportSettings = new ExportSettings(fileName, fileType);
        return (U)this;
    }

    /// <summary>
    /// Builds the console based reporter
    /// </summary>
    /// <returns></returns>
    public abstract T Build();
}

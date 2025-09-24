using Progress.Settings;

namespace Progress.Builders;

/// <summary>
/// Interface reflecting common builder methods for console based reporter builders
/// </summary>
public interface IConsoleReporterBuilder
{
    /// <summary>
    /// The reporter will display the elapsed time since the start.
    /// </summary>
    /// <returns></returns>
    IConsoleReporterBuilder DisplayingElapsedTime();

    /// <summary>
    /// The reporter will display the remaining time to finish.
    /// </summary>
    /// <returns></returns>
    IConsoleReporterBuilder DisplayingRemainingTime();

    /// <summary>
    /// The reporter will display when the operation is expected to finish.
    /// </summary>
    /// <returns></returns>
    IConsoleReporterBuilder DisplayingTimeOfArrival();

    /// <summary>
    /// The reporter will display when the operation started.
    /// </summary>
    /// <returns></returns>
    IConsoleReporterBuilder DisplayingStartingTime();

    /// <summary>
    /// The rerporter will display the total of items processed.
    /// </summary>
    /// <returns></returns>
    IConsoleReporterBuilder DisplayingItemsOverview();

    /// <summary>
    /// The reporter will display the amount of success and failures.
    /// </summary>
    /// <returns></returns>
    IConsoleReporterBuilder DisplayingItemsSummary();

    /// <summary>
    /// The reporter will hide the workload progression when it gets the 100%.
    /// </summary>
    /// <param name="hide"></param>
    /// <returns></returns>
    IConsoleReporterBuilder HideWorkloadOnComplete(bool hide);

    /// <summary>
    /// Sets the frequency the reporter will refresh the status of the operation.
    /// The default reporting frequency is set to 1s.
    /// </summary>
    /// <param name="frequency"></param>
    /// <returns></returns>
    IConsoleReporterBuilder UsingReportingFrequency(TimeSpan frequency);

    /// <summary>
    /// Sets the progress notification callback.
    /// Default notification frequency is set to 5 seconds.
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    IConsoleReporterBuilder NotifyingProgress(Action<Stats> callback);

    /// <summary>
    /// Sets the progress notification callback with the invocation frequency.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="statsFrequency"></param>
    /// <returns></returns>
    IConsoleReporterBuilder NotifyingProgress(Action<Stats> callback, TimeSpan statsFrequency);

    /// <summary>
    /// Sets the completion notification callback.
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    IConsoleReporterBuilder NotifyingCompletion(Action<Stats> callback);

    /// <summary>
    /// Sets and tells the reporter how and where to export the final stats.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="fileType"></param>
    /// <returns></returns>
    IConsoleReporterBuilder ExportingTo(string fileName, FileType fileType);
}

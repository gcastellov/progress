using Progress.Reporters;
using Progress.Settings;

namespace Progress.Builders;

/// <summary>
///  Builder for helping to set up the background reporter with the desired behavior.
/// </summary>
public class BackgroundReporterBuilder
{
    private TimeSpan _statsFrequency = TimeSpan.FromSeconds(5);
    private ExportSettings _exportSettings = default!;
    private Action<Stats> _onProgressNotified = default!;
    private Action<Stats> _onCompletionNotified = default!;

    /// <summary>
    /// Sets the progress notification callback.
    /// Default notification frequency is set to 5 seconds.
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public BackgroundReporterBuilder NotifyingProgress(Action<Stats> callback)
    {
        return NotifyingProgress(callback, _statsFrequency);
    }

    /// <summary>
    /// Sets the progress notification callback with the invocation frequency.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="statsFrequency"></param>
    /// <returns></returns>
    public BackgroundReporterBuilder NotifyingProgress(Action<Stats> callback, TimeSpan statsFrequency)
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
    public BackgroundReporterBuilder NotifyingCompletion(Action<Stats> callback)
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
    public BackgroundReporterBuilder ExportingTo(string fileName, FileType fileType)
    {
        _exportSettings = new ExportSettings(fileName, fileType);
        return this;
    }

    /// <summary>
    /// Builds the reporter getting an instance of <see cref="BackgroundReporter"/>.
    /// </summary>
    /// <param name="itemsCount"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public BackgroundReporter Build(ulong itemsCount)
    {
        if (itemsCount == 0)
            throw new ArgumentException("Nothing to do! Set the initial items count for completion.");

        var reporter = new BackgroundReporter(itemsCount)
        {
            OnProgress = _onProgressNotified,
            OnCompletion = _onCompletionNotified
        };

        reporter.Configuration.StatsFrequency = _statsFrequency;
        reporter.Configuration.ExportSettings = _exportSettings;
        reporter.Configuration.Options.NotifyProgressStats = _onProgressNotified != null;
        reporter.Configuration.Options.NotifyCompletionStats = _onCompletionNotified != null;
        reporter.Configuration.Options.ExportCompletionStats = _exportSettings != null;

        return reporter;
    }
}

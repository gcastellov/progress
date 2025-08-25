using Progress.Descriptors;

namespace Progress;

/// <summary>
/// Builder for helping to set up the reported with the desired behavior.
/// It will create an instance of <see cref="Reporter"/> using a <see cref="BarDescriptor"/> by default.
/// </summary>
public class ReporterBuilder
{
    private bool _displayElapsedTime;
    private bool _displayStartingTime;
    private bool _displayItemsOverview;
    private bool _displayItemsSummary;
    private TimeSpan _reportFrequency = TimeSpan.FromSeconds(1);
    private ComponentDescriptor _componentDescriptor = BarDescriptor.Default;

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
            DisplayElapsedTime = _displayElapsedTime,
            DisplayStartingTime = _displayStartingTime,
            DisplayItemsOverview = _displayItemsOverview,
            DisplayItemsSummary = _displayItemsSummary,
            ReportFrequency = _reportFrequency,
        };
    }
}

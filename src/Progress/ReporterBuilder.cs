using Progress.Descriptors;

namespace Progress;

public class ReporterBuilder
{
    private bool _displayElapsedTime;
    private bool _displayStartingTime;
    private bool _displayItemsOverview;
    private bool _displayItemsSummary;
    private TimeSpan _reportFrequency = TimeSpan.FromSeconds(1);
    private ComponentDescriptor _componentDescriptor = BarDescriptor.Default;

    public ReporterBuilder DisplayingElapsedTime()
    {
        _displayElapsedTime = true;
        return this;
    }

    public ReporterBuilder DisplayingStartingTime()
    {
        _displayStartingTime = true;
        return this;
    }

    public ReporterBuilder DisplayingItemsOverview()
    {
        _displayItemsOverview = true;
        return this;
    }

    public ReporterBuilder DisplayingItemsSummary()
    {
        _displayItemsSummary = true;
        return this;
    }

    public ReporterBuilder UsingReportingFrequency(TimeSpan frequency)
    {
        _reportFrequency = frequency;
        return this;
    }

    public ReporterBuilder UsingComponentDescriptor(ComponentDescriptor descriptor)
    {
        _componentDescriptor = descriptor;
        return this;
    }

    public Reporter Build(ulong itemsCount)
    {
        var component = _componentDescriptor.Build(itemsCount);
        
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

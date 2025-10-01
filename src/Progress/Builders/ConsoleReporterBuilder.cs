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

        if (_componentDescriptor == null)
            throw new ArgumentException("Add a component descriptor to display progress");

        var component = _componentDescriptor.Build();

        var workload = Workload.Default(_expectedItemsCount);
        workload.Component = component;

        ConsoleReporter reporter = new(workload);
        SetCallbacks(reporter);
        SetConfiguration(reporter);

        return reporter;
    }
}

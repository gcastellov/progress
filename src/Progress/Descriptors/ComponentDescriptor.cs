using Progress.Components;

namespace Progress.Descriptors;

/// <summary>
/// The component descriptor base class.
/// </summary>
public abstract class ComponentDescriptor
{
    internal bool DisplayPercent { get; private set; }

    internal ComponentDescriptor DisplayingPercent()
    {
        DisplayPercent = true;
        return this;
    }

    internal abstract Component Build();
}

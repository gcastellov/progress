using Progress.Components;
namespace Progress.Descriptors;

public abstract class ComponentDescriptor
{
    protected bool DisplayPercent { get; private set; }

    public ComponentDescriptor DisplayingPercent()
    {
        DisplayPercent = true;
        return this;
    }

    internal abstract IComponent Build(ulong availableItems);
}

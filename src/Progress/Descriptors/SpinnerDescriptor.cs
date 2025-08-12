using Progress.Components;

namespace Progress.Descriptors;

public class SpinnerDescriptor : ComponentDescriptor
{
    public static SpinnerDescriptor Default => new SpinnerDescriptor().DisplayingPercent();

    public new SpinnerDescriptor DisplayingPercent() => (SpinnerDescriptor)base.DisplayingPercent();

    internal override IComponent Build(ulong availableItems)
    {
        return new Spinner() { DisplayPercent = DisplayPercent };
    }
}
